package com.futureruler.uframework.obbdownloader.core.download;

import com.futureruler.uframework.obbdownloader.core.ObbInfo;
import com.futureruler.uframework.obbdownloader.core.ObbStatus;
import com.futureruler.uframework.obbdownloader.core.common.XAPKFile;
import com.futureruler.uframework.obbdownloader.core.util.ResourceUtil;
import com.google.android.vending.expansion.downloader.DownloadProgressInfo;
import com.google.android.vending.expansion.downloader.DownloaderClientMarshaller;
import com.google.android.vending.expansion.downloader.DownloaderServiceMarshaller;
import com.google.android.vending.expansion.downloader.Helpers;
import com.google.android.vending.expansion.downloader.IDownloaderClient;
import com.google.android.vending.expansion.downloader.IDownloaderService;
import com.google.android.vending.expansion.downloader.IStub;

import android.app.Activity;
import android.app.PendingIntent;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Messenger;
import android.util.Log;
import android.view.WindowManager;

public class ObbDownloadHelper implements IDownloaderClient {

    private Context mContext;
    private boolean mNativeUI;
    private ObbDownloadListener mListener;
    private IStub mDownloaderClientStub;
    private IDownloaderService mRemoteService;
    private MyProgressDialog mDownloadProgressDlg;
    private XAPKFile[] xAPKS;
    private boolean mIsConnected;
    private boolean mIsFinished;
    private boolean mIsDownloadPaused;

    public ObbDownloadHelper(Context context, ObbInfo obbInfo, boolean nativeUI) {
        mContext = context;
        mNativeUI = nativeUI;
        mIsConnected = false;
        mIsFinished = false;
        mIsDownloadPaused = false;
        int mainVer = obbInfo.getMainObbVersion();
        long mainSize = obbInfo.getMainObbFileSize();
        int patchVer = obbInfo.getPatchObbVersion();
        long patchSize = obbInfo.getPatchObbFileSize();
// 原始版本
//        if (mainVer > 0 && patchVer > 0) {
//            if (mainSize > 0 && patchSize > 0) {
//                xAPKS = new XAPKFile[2];
//                xAPKS[0] = new XAPKFile(true, mainVer, mainSize);
//                xAPKS[1] = new XAPKFile(false, patchVer, patchSize);
//            } else {
//                xAPKS = new XAPKFile[1];
//                xAPKS[0] = new XAPKFile(true, mainVer, mainSize);
//            }
//        } else {
//            xAPKS = new XAPKFile[0];
//        }

        // 修复后版本
        if (mainVer > 0 && patchVer > 0) {
            xAPKS = new XAPKFile[2];
            xAPKS[0] = new XAPKFile(true, mainVer, mainSize);
            xAPKS[1] = new XAPKFile(false, patchVer, patchSize);
        } else if (mainVer > 0) {
            xAPKS = new XAPKFile[1];
            xAPKS[0] = new XAPKFile(true, mainVer, mainSize);
        } else if (patchVer > 0) {
            xAPKS = new XAPKFile[1];
            xAPKS[1] = new XAPKFile(false, patchVer, patchSize);
        } else {
            xAPKS = new XAPKFile[0];
        }
        ObbDownloadService.BASE64_PUBLIC_KEY = obbInfo.getPublicKey();
        ObbDownloadService.SALT = obbInfo.getSalt();
    }

    /**
     * Go through each of the Expansion APK files defined in the project and
     * determine if the files are present and match the required size. Free
     * applications should definitely consider doing this, as this allows the
     * application to be launched for the first time without having a network
     * connection present. Paid applications that use LVL should probably do at
     * least one LVL check that requires the network to be present, so this is
     * not as necessary.
     *
     * @return true if they are present.
     */
    public boolean expansionFilesDelivered() {
        // Determine whether the apk expansion files exist, and the file size is
        // correct
        for (XAPKFile xf : xAPKS) {
            String fileName = Helpers.getExpansionAPKFileName(mContext, xf.mIsMain, xf.mFileVersion);
            if (!Helpers.doesFileExist(mContext, fileName, xf.mFileSize, false)) {
                if (xf.mIsMain) {
                    Log.i("APKExpansionDownloader", "Expansion files are not delivered: main obb does not exist");
                } else {
                    Log.i("APKExpansionDownloader", "Expansion files are not delivered: patch obb does not exist");
                }
                return false;
            }
        }
        Log.i("APKExpansionDownloader", "Expansion files are already delivered");
        return true;
    }

    public void downloadExpansionFiles(Activity activity, ObbDownloadListener listener) {
        try {
            mListener = listener;
            mIsFinished = false;
            // Build an Intent to start this activity from the Notification
            Intent notifierIntent = new Intent(activity, activity.getClass());
            notifierIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP);
            PendingIntent pendingIntent = PendingIntent.getActivity(activity, 0, notifierIntent,
                    PendingIntent.FLAG_UPDATE_CURRENT);

            // Start the download service (if required)
            int startResult = DownloaderClientMarshaller.startDownloadServiceIfRequired(activity, pendingIntent,
                    ObbDownloadService.class);
            // If download has started, initialize a ProgressDialog to show
            // download progress
            if (startResult != DownloaderClientMarshaller.NO_DOWNLOAD_REQUIRED) {
                // This is where you do set up to display the download
                // progress (next step)
                // Instantiate a member instance of IStub
                Log.i("APKExpansionDownloader", "Try to download obb files");
                mDownloaderClientStub = DownloaderClientMarshaller.CreateStub(this, ObbDownloadService.class);
                // Create and show the progress dialog
                if (mNativeUI) {
                    mDownloadProgressDlg = new MyProgressDialog(activity);
                    mDownloadProgressDlg.show();
                }
            }
            // No need to download obb files
            else {
                Log.i("APKExpansionDownloader", "No need to download obb files");
                onDownloadError(ObbStatus.NoDownloadRequired);
            }
        } catch (NameNotFoundException e) {
            e.printStackTrace();
            onDownloadFailed();
        }
    }

    /*
     * connect to the download service
     */
    public void connect() {
        if (mDownloaderClientStub != null && !mIsFinished && !mIsConnected) {
            Log.i("APKExpansionDownloader", "Connect to download service.");
            mDownloaderClientStub.connect(mContext);
            mIsConnected = true;
        }
    }

    /*
     * disconnect from the download service after download process finished,
     * whether success or failure, disconnect should be called otherwise a
     * leaked may produced
     */
    public void disconnect() {
        if (mDownloaderClientStub != null && !mIsFinished && mIsConnected) {
            Log.i("APKExpansionDownloader", "Disconnect from download service.");
            mDownloaderClientStub.disconnect(mContext);
            mIsConnected = false;
        }
    }

    /**
     * 继续下载
     */
    public void continueDownload() {
        if (mIsFinished || mRemoteService == null) {
            return;
        }
        if (mIsDownloadPaused) {
            mRemoteService.requestContinueDownload();
            onContinueDownload();
        }
    }

    private void onContinueDownload() {
        mIsDownloadPaused = false;

        if (mListener != null) {
            mListener.onPause(false);
        }
    }

    /**
     * 暂停下载
     */
    public void pauseDownload() {
        if (mIsFinished || mRemoteService == null) {
            return;
        }
        if (!mIsDownloadPaused) {
            mRemoteService.requestPauseDownload();
            onPauseDownload();
        }
    }

    private void onPauseDownload() {
        mIsDownloadPaused = true;

        if (mListener != null) {
            mListener.onPause(true);
        }
    }

    /**
     * 取消下载
     */
    public void abortDownload() {
        if (mRemoteService != null && !mIsFinished) {
            mRemoteService.requestAbortDownload();

            onAbortDownload();
        }
    }

    private void onAbortDownload() {
        if (!mIsFinished) {
            disconnect();
            if (mListener != null) {
                mListener.onAbort();
            }
            mIsFinished = true;
        }
    }

    private void onProgressDownload(int progress) {
        if (!mIsFinished) {
            if (mListener != null) {
                mListener.onProgress(progress);
            }
        }
    }

    private void onDownloadSuccess() {
        if (!mIsFinished) {
            disconnect();
            if (mListener != null) {
                mListener.onSuccess();
            }
            mIsFinished = true;
        }
    }

    private void onDownloadFailed() {
        if (!mIsFinished) {
            disconnect();
            if (mListener != null) {
                mListener.onFailed();
            }
            mIsFinished = true;
        }
    }

    private void onDownloadError(int errorCode) {
        if (!mIsFinished) {
            disconnect();
            if (mListener != null) {
                mListener.onError(errorCode);
            }
            mIsFinished = true;
        }
    }

    @Override
    public void onServiceConnected(Messenger m) {
        mRemoteService = DownloaderServiceMarshaller.CreateProxy(m);
        mRemoteService.onClientUpdated(mDownloaderClientStub.getMessenger());
        Log.i("APKExpansionDownloader", "ServiceConnected");
    }

    @Override
    public void onDownloadStateChanged(int newState) {
        Log.i("APKExpansionDownloader", "DownloadStateChanged: " + newState);
        switch (newState) {
            // Download success
            case IDownloaderClient.STATE_COMPLETED:
                if (mNativeUI) {
                    mDownloadProgressDlg.success();
                } else {
                    onProgressDownload(100);
                    onDownloadSuccess();
                }
                break;
            // Download failure
            case IDownloaderClient.STATE_FAILED_CANCELED:
            case IDownloaderClient.STATE_FAILED_FETCHING_URL:
            case IDownloaderClient.STATE_FAILED_UNLICENSED:
            case IDownloaderClient.STATE_FAILED:
                if (mNativeUI) {
                    mDownloadProgressDlg.failed();
                } else {
                    onDownloadFailed();
                }
                mIsDownloadPaused = true;
                break;
            // Download paused
            case IDownloaderClient.STATE_PAUSED_NEED_CELLULAR_PERMISSION:
            case IDownloaderClient.STATE_PAUSED_WIFI_DISABLED_NEED_CELLULAR_PERMISSION:
            case IDownloaderClient.STATE_PAUSED_BY_REQUEST:
            case IDownloaderClient.STATE_PAUSED_ROAMING:
            case IDownloaderClient.STATE_PAUSED_SDCARD_UNAVAILABLE:
                if (mNativeUI) {
                    mDownloadProgressDlg.paused();
                } else {
                    onPauseDownload();
                }
                mIsDownloadPaused = true;
                break;
            // Download resume (do nothing)
            case IDownloaderClient.STATE_IDLE:
            case IDownloaderClient.STATE_CONNECTING:
            case IDownloaderClient.STATE_FETCHING_URL:
            case IDownloaderClient.STATE_DOWNLOADING:
                break;
            // unknown state (do nothing)
            default:
                break;
        }
    }

    @Override
    public void onDownloadProgress(DownloadProgressInfo progress) {
        int progressInPercent = (int) (progress.mOverallProgress * 100 / progress.mOverallTotal);
        Log.i("APKExpansionDownloader", "DownloadProgress: " + progressInPercent);
        if (mNativeUI) {
            mDownloadProgressDlg.setProgress(progressInPercent);
        }
        onProgressDownload(progressInPercent);
    }

    // Custom progress dialog
    private class MyProgressDialog extends ProgressDialog {
        private boolean mIsPaused; // Indicates whether the download process is
        // paused
        private boolean mIsComplete; // Indicates whether the download process
        // is complete
        private String mDlgTitleDownloading; // Dialog title when downloading
        private String mDlgTitleFailed; // Dialog title when download failed
        private String mDlgTitlePaused; // Dialog title when download paused
        private String mDlgTitleComplete; // Dialog title when download complete
        private String mDlgCancelBtnText; // Dialog cancel button text
        private String mDlgResumeBtnText; // Dialog resume button text
        private String mDlgPauseBtnText; // Dialog pause button text
        private String mDlgRetryBtnText; // Dialog retry button text

        public MyProgressDialog(Context context) {
            super(context);
            mIsPaused = false;
            mIsComplete = false;
            mDlgTitleDownloading = ResourceUtil.getString(mContext, "obb_download_title_downloading");
            mDlgTitleFailed = ResourceUtil.getString(mContext, "obb_download_title_failed");
            mDlgTitlePaused = ResourceUtil.getString(mContext, "obb_download_title_paused");
            mDlgTitleComplete = ResourceUtil.getString(mContext, "obb_download_title_complete");
            mDlgCancelBtnText = ResourceUtil.getString(mContext, "obb_download_btn_cancel");
            mDlgPauseBtnText = ResourceUtil.getString(mContext, "obb_download_btn_pause");
            mDlgResumeBtnText = ResourceUtil.getString(mContext, "obb_download_btn_resume");
            mDlgRetryBtnText = ResourceUtil.getString(mContext, "obb_download_btn_retry");
            setIndeterminate(false);
            setCanceledOnTouchOutside(false);
            setCancelable(false);
            setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);
            setIcon(ResourceUtil.getResourceId(mContext, "obb_downloader_indicator", "drawable"));
            getWindow().setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
                    WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
            getWindow().addFlags(
                    WindowManager.LayoutParams.FLAG_ALT_FOCUSABLE_IM | WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
            setTitle(mDlgTitleDownloading);
            setProgress(0);
            setMax(100);

            setButton(DialogInterface.BUTTON_POSITIVE, mDlgPauseBtnText, new OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    // If download is already complete, do nothing.
                    if (mIsComplete) {
                        return;
                    }
                    // Under certain circumstance, onServiceConnected() is not
                    // called, and mRemoteService is null
                    // So a null check is necessary
                    if (mRemoteService != null) {
                        // Current download state is paused.
                        // A click on this button would trigger a continue
                        // download request
                        if (mIsPaused) {
                            mRemoteService.requestContinueDownload();
                            setTitle(mDlgTitleDownloading);
                            getButton(DialogInterface.BUTTON_POSITIVE).setText(mDlgPauseBtnText);
                            onContinueDownload();
                            Log.i("APKExpansionDownloader", "download continued by user");
                        }
                        // Current download state is downloading.
                        // A click on this button would trigger a pause download
                        // request
                        else {
                            mRemoteService.requestPauseDownload();
                            setTitle(mDlgTitlePaused);
                            getButton(DialogInterface.BUTTON_POSITIVE).setText(mDlgResumeBtnText);
                            onPauseDownload();
                            Log.i("APKExpansionDownloader", "download paused by user");
                        }
                        mIsPaused = !mIsPaused;
                    }
                }
            });

            setButton(DialogInterface.BUTTON_NEGATIVE, mDlgCancelBtnText, new OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    // If download is already complete, do nothing.
                    if (mIsComplete) {
                        return;
                    }
                    // Stop the download service
                    if (mRemoteService != null) {
                        mRemoteService.requestAbortDownload();
                        Log.i("APKExpansionDownloader", "download canceled by user");
                    }
                    // Dismiss the dialog
                    trueDismiss();
                    // Inform the download has been canceled
                    onAbortDownload();
                }
            });
        }

        // In order to prevent close of the dialog when click BUTTON POSITIVE,
        // we override dismiss() and do nothing here.
        // If you want close the dialog manually, call trueDismiss()
        @Override
        public void dismiss() {
            // do nothing
        }

        private void trueDismiss() {
            super.dismiss();
        }

        public void success() {
            // Sometimes, success is called more than once after download
            // complete.
            // So a check is necessary
            if (!mIsComplete) {
                setProgress(100);
                onProgressDownload(100);
                setTitle(mDlgTitleComplete);
                // Close the dialog after two seconds
                getWindow().getDecorView().postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        trueDismiss();
                        onDownloadSuccess();
                    }
                }, 2000);
                mIsComplete = true;
                Log.i("APKExpansionDownloader", "Download success");
            }
        }

        public void failed() {
            setTitle(mDlgTitleFailed);
            getButton(DialogInterface.BUTTON_POSITIVE).setText(mDlgRetryBtnText);
            mIsPaused = true;
        }

        public void paused() {
            setTitle(mDlgTitlePaused);
            getButton(DialogInterface.BUTTON_POSITIVE).setText(mDlgResumeBtnText);
            mIsPaused = true;
        }
    }
}
