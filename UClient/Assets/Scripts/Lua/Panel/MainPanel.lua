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
    
    -- pbc(GS)
    local on_click_gsSendPBC = function()
        local protobuf = require("3rd.pbc.protobuf")

        local addressbook = {
            name = "Client",
            id = 12345,
            phones = {
                {number = "1301234567"},
                {number = "87654321", type = "WORK"}
            }
        }
        NetManager:sendGamePBC(1001, addressbook)
    end
    self:_bindClick(self.view._gsSendPBC, on_click_gsSendPBC)

    -- 连接(BS)
    local on_click_bsConnect = function()
        NetManager:connect(NETWORK_CHANNEL_TYPE.BATTLE, "127.0.0.1", 15940)
    end
    self:_bindClick(self.view._bsConnect, on_click_bsConnect)

    -- pb(BS)
    local on_click_bsSendPBC = function()
        local protobuf = require("3rd.pbc.protobuf")

        local addressbook = {
            name = "Client",
            id = 12345,
            phones = {
                {number = "1301234567"},
                {number = "87654321", type = "WORK"}
            }
        }
        NetManager:sendBattlePBC(1001, addressbook)
    end
    self:_bindClick(self.view._bsSendPBC, on_click_bsSendPBC)
end

function panel:onHide()
end

function panel:onNetReceived(cmd, pack)
    if cmd == 1001 then
        logger.debug("收到了1001测试协议")
    end
end

return panel
