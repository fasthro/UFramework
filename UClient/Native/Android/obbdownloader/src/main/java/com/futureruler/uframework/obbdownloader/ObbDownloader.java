package com.futureruler.uframework.obbdownloader;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

import com.futureruler.uframework.obbdownloader.core.ObbHelper;
import com.futureruler.uframework.obbdownloader.core.ObbInfo;
import com.futureruler.uframework.obbdownloader.core.ObbStatus;
import com.futureruler.uframework.obbdownloader.core.download.ObbDownloadListener;

// obb file download helper

public class ObbDownloader {
    // Context
    static Context mainContext = null;

    // google public key
    static String PUBLIC_KEY = "REPLACE THIS WITH YOUR PUBLIC KEY - DONE FROM C#";

    // obb version
    static int MAIN_OBB_VERSION = 0;
    static int PATCH_OBB_VERSION = 0;

    // used by the preference obfuscater
    static byte[] SALT = new byte[]{
            1, 43, -12, -1, 54, 98,
            -100, -12, 43, 2, -8, -4, 9, 5, -106, -108, -33, 45, -1, 84
    };

    // obb helper
    static ObbHelper obbHelper;

    // native ui
    static boolean nativeUI = true;

    // 下载接口
    static ObbDownloadListener obbDownloadListener;

    /**
     * 初始化
     *
     * @param context
     * @param isNativeUI 是否使用原生界面
     */
    public static void initialize(Context context, boolean isNativeUI) {
        mainContext = context;
        nativeUI = isNativeUI;
        obbHelper = new ObbHelper(context, new ObbInfo() {

            // This function must be override to return your app's public key
            @Override
            public String getPublicKey() {
                return PUBLIC_KEY;
            }

            // This function must be override to return the main obb version
            // The returned version must be greater than 0
            @Override
            public int getMainObbVersion() {
                return MAIN_OBB_VERSION;
            }

            // This function must be override to return the main obb file size
            // The returned size must be greater than 0
            @Override
            public long getMainObbFileSize() {
                return 1546530L;
            }
        }, nativeUI);
    }

    /**
     * get context
     *
     * @return
     */
    public static Context getContext() {
        return mainContext;
    }

    /**
     * get activity
     *
     * @return
     */
    public static Activity getActivity() {
        return (Activity) mainContext;
    }

    /**
     * 显示 Toast 提示
     *
     * @param text
     */
    public static void showToast(String text) {
        Toast.makeText(mainContext, text, Toast.LENGTH_SHORT).show();
    }

    /**
     * connect
     */
    public static void connect() {
        obbHelper.connect();
    }

    /**
     * disconnect
     */
    public static void disconnect() {
        obbHelper.disconnect();
    }

    /**
     * 设置下载回调接口
     *
     * @param listener
     */
    public static void setDownloadListener(ObbDownloadListener listener) {
        obbDownloadListener = listener;
    }

    /**
     * 下载扩展文件
     */
    public static void downloadExpansion() {
        if (!obbHelper.expansionFilesDelivered()) {
            // The obb files haven't delivered, so we should download the obb files.
            obbHelper.downloadExpansionFiles(getActivity(), new ObbDownloadListener() {
                @Override
                public void onProgress(int progress) {
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onProgress(progress);
                    }
                }

                @Override
                public void onSuccess() {
                    if (nativeUI) {
                        showToast("download success.");
                    }
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onSuccess();
                    }
                }

                @Override
                public void onFailed() {
                    if (nativeUI) {
                        showToast("download failed.");
                    }
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onFailed();
                    }
                }

                @Override
                public void onPause(boolean pause) {
                    if (nativeUI) {
                        if (pause) {
                            showToast("download paused.");
                        } else {
                            showToast("download continue.");
                        }
                    }
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onPause(pause);
                    }
                }

                @Override
                public void onAbort() {
                    if (nativeUI) {
                        showToast("download abort.");
                    }
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onAbort();
                    }
                }

                @Override
                public void onError(int errorCode) {
                    if (nativeUI) {
                        showToast("errorCode: " + errorCode);
                    }
                    if (obbDownloadListener != null) {
                        obbDownloadListener.onError(errorCode);
                    }
                }
            });

            obbHelper.connect();
        } else {
            if (nativeUI) {
                showToast("Expansion files are already delivered.");
            }
            if (obbDownloadListener != null) {
                obbDownloadListener.onError(ObbStatus.NoDownloadRequired);
            }
        }
    }

    /**
     * 继续下载扩展文件
     */
    public static void continueDownload() {
        obbHelper.continueDownload();
    }

    /**
     * 暂停下载扩展文件
     */
    public static void pauseDownload() {
        obbHelper.pauseDownload();
    }

    /**
     * 取消下载扩展文件
     */
    public static void abortDownload() {
        obbHelper.abortDownload();
    }
}
