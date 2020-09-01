package com.futureruler.uframework.obbdownloader.core.download;

public interface ObbDownloadListener {
    void onProgress(int progress);
    void onSuccess();
    void onFailed();
    void onPause(boolean pause);
    void onAbort();
    void onError(int errorCode);
}
