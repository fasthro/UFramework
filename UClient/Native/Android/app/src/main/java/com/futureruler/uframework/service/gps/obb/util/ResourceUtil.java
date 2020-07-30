package com.futureruler.uframework.service.gps.obb.util;

import android.content.Context;
import android.util.Log;

public class ResourceUtil {
    public static int getResourceId(Context context, String resName, String resType) {
        return context.getResources().getIdentifier(resName, resType, context.getPackageName());
    }

    public static String getString(Context context, String resName) {
        Log.d("UFramework", "----------------------->> " + resName);
        return context.getString(getResourceId(context, resName, "string"));
    }
}
