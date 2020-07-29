package com.futureruler.uframework;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

import com.futureruler.uframework.service.device.DeviceCountry;
import com.futureruler.uframework.service.device.DeviceId;
import com.futureruler.uframework.service.gps.obb.ObbHelper;
import com.futureruler.uframework.service.gps.obb.ObbHelperListener;
import com.futureruler.uframework.service.gps.obb.ObbInfo;

/**
 * @Author: fasthro
 * @Date: 2020/7/25 1:23
 * @Description: uframework native
 */

public class MainNative {

    // Context
    private static Context mainContext = null;

    // GPS
    // public key
    private static String gpsPublicKey = null;

    // obb
    private static ObbHelper gpsObbHelper = null;

    /**
     * 初始化
     *
     * @param context
     */
    public static void initialize(Context context) {
        mainContext = context;
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

    /*************************************************************/
    /////////////////////////// Device ///////////////////////////
    /*************************************************************/

    /**
     * 获取设备唯一标识
     *
     * @return
     */
    public static String getDeviceId() {
        return DeviceId.get(mainContext);
    }

    /**
     * 获取设备国家ISO
     *
     * @return
     */
    public static String getDeviceCountry() {
        return DeviceCountry.get(mainContext);
    }

    /*************************************************************/
    ///////////////////// Google Play Service ////////////////////
    /*************************************************************/

    /**
     * 设置 Google Play Public Key
     *
     * @param key
     */
    public static void setGPSPublicKey(String key) {
        gpsPublicKey = key;
    }

    /**
     * 获取 Google Play Key
     *
     * @return
     */
    public static String getGPSPublicKey() {
        return gpsPublicKey;
    }

    /**
     * 检查 Google Play Obb
     */
    public static void checkGPSObb() {
        gpsObbHelper = new ObbHelper(mainContext, new ObbInfo() {

            // This function must be override to return your app's public key
            @Override
            public String getPublicKey() {
                return gpsPublicKey;
            }

            // This function must be override to return the main obb version
            // The returned version must be greater than 0
            @Override
            public int getMainObbVersion() {
                return 3;
            }

            // This function must be override to return the main obb file size
            // The returned size must be greater than 0
            @Override
            public long getMainObbFileSize() {
                return 1546530L;
            }

            // If you donn't have patch obb file, you don't need override this function
            @Override
            public int getPatchObbVersion() {
                return 3;
            }

            // If you donn't have patch obb file, you don't need override this function
            @Override
            public long getPatchObbFileSize() {
                return 5570L;
            }
        });

        // Check whether the obb files are delivered.
        if (!gpsObbHelper.expansionFilesDelivered()) {
            // The obb files haven't delivered, so we should download the obb files.
            gpsObbHelper.downloadExpansionFiles(getActivity(), new ObbHelperListener() {
                @Override
                public void onSuccess() {
                    showToast("Download success.");
                    // The obb files have been download, you can use them directly.
                    // Also you can unzip or copy them to a target folder.
                    String folder = mainContext.getExternalFilesDir(null).toString();
                    gpsObbHelper.unzipMainobbToFolder(folder, new ObbHelperListener() {
                        @Override
                        public void onSuccess() {
                            showToast("Unzip main obb file success.");
                        }

                        @Override
                        public void onFailed() {
                            showToast("Unzip main obb file failed.");
                        }
                    });
                    gpsObbHelper.copyPatchobbToFolder(folder, new ObbHelperListener() {
                        @Override
                        public void onSuccess() {
                            showToast("Copy patch obb file success.");
                        }

                        @Override
                        public void onFailed() {
                            showToast("Copy patch obb file failed.");
                        }
                    });
                }

                @Override
                public void onFailed() {
                    showToast("Download failed.");
                }
            });
        }
        // The obb files have already delivered.
        else {
            showToast("Expansion files are already delivered.");
        }
    }
}