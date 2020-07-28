package com.futureruler.uframework.service.gps.obb.download;

public interface ObbDownloadListener {
    void onDownloadComplete();

    void onDownloadFailed();
}
