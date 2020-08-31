package com.futureruler.uframework.obbdownloader;

// obb download listener

public interface UObbDownloadListener {
    void onProgress(int progress);
    void onSuccess();
    void onFailed(int errorCode);
}
