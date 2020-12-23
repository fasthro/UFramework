local panel =
    typesys.def.MainPanel {
    __super = typesys.BasePanel
}

function panel:__ctor()
    self._name = "Main"
    self._package = "Main"
    self._layer = PANEL_LAYER.PANEL
    self._dependences = nil

    panel.__super.__ctor(self)
end

function panel:onAwake()
end

function panel:onDispose()
end

function panel:onShow()
    panel.__super.onShow(self)

    EventManager:once(EVENT_NAMES.NET_CONNECTED, self.onConnected, self)

    -- 连接战斗服
    NetManager:connect(NETWORK_CHANNEL_TYPE.BATTLE, "127.0.0.1", 15940)
end

function panel:onHide()
end

function panel:onConnected()
    NetManager:sendBattlePBC(901, {uid = 1})
end

function panel:onNetReceived(cmd, pack)
    if cmd <= 0 then
        return
    end

    local cm = self["NetCmd_" .. tostring(cmd)]
    if cm ~= nil then
        cm(self, pack)
    end
end

function panel:NetCmd_901(msg)
    if msg.resultCode == 200 then
        NetManager:sendBattlePBC(902, {roomSecretKey = msg.roomSecretKey})
    end
end

function panel:NetCmd_902(msg)
    if msg.resultCode == 200 then
        logger.debug("进入房间成功，等待其他玩家加入")
    end
end

return panel
