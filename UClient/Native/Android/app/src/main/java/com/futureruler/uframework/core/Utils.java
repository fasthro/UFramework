package com.futureruler.uframework.core;

import android.content.Intent;
import com.futureruler.uframework.App;

public class Utils {
    /**
     * 重启应用
     */
    public static void restart() {
        new Thread() {
            public void run() {
                Intent launch = App.getActivity().getPackageManager().getLaunchIntentForPackage(App.getActivity().getPackageName());
                launch.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                App.getActivity().startActivity(launch);
                android.os.Process.killProcess(android.os.Process.myPid());
            }
        }.start();
        App.getActivity().finish();
    }
}
