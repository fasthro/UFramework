package com.futureruler.uframework.obbdownloader.core.download;

public interface ObbDownloadListener {
    void onProgress(int progress);
    void onDownloadComplete();
    void onDownloadFailed();
}
