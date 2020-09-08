package com.futureruler.uframework;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

import com.futureruler.uframework.core.DeviceCountry;
import com.futureruler.uframework.core.DeviceId;

/**
 * @Author: fasthro
 * @Date: 2020/7/25 1:23
 * @Description: uframework native
 */

public class App {

    // Context
    private static Context mainContext = null;

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
}