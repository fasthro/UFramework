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
    
    local on_click_pbc = function()
        local protobuf = require("3rd.pbc.protobuf")

        local addressbook = {
            name = "Alice",
            id = 12345,
            phones = {
                {number = "1301234567"},
                {number = "87654321", type = "WORK"}
            }
        }
        NetManager:sendPBC(1001, addressbook)
    end
    self:_bindClick(self.view._sendpbc, on_click_pbc)
end

function panel:onHide()
end

function panel:onNetReceived(pack)
end

return panel
