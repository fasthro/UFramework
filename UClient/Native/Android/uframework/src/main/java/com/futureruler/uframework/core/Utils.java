package com.futureruler.uframework.core;

import android.annotation.SuppressLint;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.ClipData;
import android.content.ClipDescription;
import android.content.ClipboardManager;
import android.content.Intent;
import android.os.Build;

import com.futureruler.uframework.App;

public class Utils {
    // 剪切板
    static ClipboardManager clipboard = null;

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

    /**
     * 复制文本到剪切板
     *
     * @param str
     * @throws Exception
     */
    @TargetApi(Build.VERSION_CODES.HONEYCOMB)
    @SuppressLint("NewApi")
    public static void setClipBoard(final String str) throws Exception {
        clipboard = (ClipboardManager) App.getContext().getSystemService(Activity.CLIPBOARD_SERVICE);
        ClipData cd = ClipData.newPlainText("text", str);
        clipboard.setPrimaryClip(cd);
    }

    /**
     * 从剪切板获取文本
     *
     * @return
     */
    @TargetApi(Build.VERSION_CODES.HONEYCOMB)
    @SuppressLint("NewApi")
    public static String getClipBoard() {
        if (clipboard != null && clipboard.hasPrimaryClip()
                && clipboard.getPrimaryClipDescription().hasMimeType(ClipDescription.MIMETYPE_TEXT_PLAIN)) {
            ClipData cdText = clipboard.getPrimaryClip();
            ClipData.Item item = cdText.getItemAt(0);
            return item.getText().toString();
        }
        return "";
    }
}
