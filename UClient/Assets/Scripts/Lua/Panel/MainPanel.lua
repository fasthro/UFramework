local CMD = {
    [901] = true,
    [902] = true,
    [903] = true
}

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
    NetManager:connect(NETWORK_CHANNEL_TYPE.BATTLE, ServerManager.battle_server_ip, ServerManager.battle_server_port)
end

function panel:onHide()
end

function panel:onConnected()
    NetManager:sendBattlePBC(901, {uid = 1})
end

function panel:onNetReceived(cmd, pack)
    if cmd <= 0 or not CMD[cmd] then
        return
    end

    local cm = self["NetCmd_" .. tostring(cmd)]
    if cm ~= nil then
        cm(self, pack)
    end
end

function panel:NetCmd_901(msg)
    NetManager:sendBattlePBC(902, {})
end

function panel:NetCmd_902(msg)
    NetManager:sendBattlePBC(903, {uid = 1})
end

function panel:NetCmd_903(msg)
    logger.debug("已经准备完成")
end

return panel
