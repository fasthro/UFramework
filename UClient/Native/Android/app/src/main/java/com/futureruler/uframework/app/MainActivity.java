package com.futureruler.uframework.app;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;

import com.futureruler.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;

// 继承 UnityPlayerActivity
// 方便开发扩展
public class MainActivity extends UnityPlayerActivity {

    // 是否启动授权提示
    static boolean AUTHORIZED_ENABLED = true;
    // 授权权限
    static String AUTHORIZED_PERMISSION = "android.permission.READ_EXTERNAL_STORAGE";
    // 授权提示标题
    static String AUTHORIZED_TITLE = "Permission Required";
    // 授权提示内容
    static String AUTHORIZED_MESSAGE = "Greetings Governor, the game requires some permissions to download expansion resource files and save your game progress. Please grant the permission when promoted. If denied, the game may not start properly. You can also grant the permission in Settings->Apps, then find the game and grant the permission. Have fun with Rise of Warfare!";
    // 授权提示按钮标题
    static String AUTHORIZED_BUTTON_TITLE = "Got it";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        if (AUTHORIZED_ENABLED) {
            checkPermission();
        }
        super.onCreate(savedInstanceState);
    }

    /**
     * 授权提示
     */
    private void checkPermission() {
        if (ContextCompat.checkSelfPermission(this, AUTHORIZED_PERMISSION) == 0)
            return;
        if (!ActivityCompat.shouldShowRequestPermissionRationale(this, AUTHORIZED_PERMISSION)) {
            AlertDialog.Builder alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.setCancelable(false);
            alertBuilder.setTitle(AUTHORIZED_TITLE);
            alertBuilder.setMessage(AUTHORIZED_MESSAGE);
            alertBuilder.setPositiveButton(AUTHORIZED_BUTTON_TITLE, new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    ActivityCompat.requestPermissions(UnityPlayer.currentActivity,
                            new String[]{AUTHORIZED_PERMISSION},
                            100);
                }
            });
            AlertDialog alert = alertBuilder.create();
            alert.show();
        } else {
            ActivityCompat.requestPermissions(this,
                    new String[]{AUTHORIZED_PERMISSION},
                    100);
        }
    }

    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        switch (requestCode) {
            case 100:
                if (grantResults.length > 0) {
                    //grantResults[0];
                }
                return;
        }
    }
}
