local panel =
    typesys.def.LoginPanel {
    __super = typesys.BasePanel
}

function panel:__ctor()
    self._name = "Login"
    self._package = "Login"
    self._layer = PANEL_LAYER.PANEL
    self._dependences = nil

    panel.__super.__ctor(self)
end

function panel:_onAwake()
end

function panel:_onDispose()
end

function panel:onShow()
    panel.__super.onShow(self)

    self.view._username._input.text = "fasthro"
    self.view._password._input.text = "password"

    local on_click_login = function()
        local username = self.view._username._input.text
        local password = self.view._password._input.text
        local serverid = "1"
        LoginCtrl:login(username, password, serverid)
    end
    self:_bindClick(self.view._login, on_click_login)
end

function panel:onHide()
end

function panel:onEventReceive(id)
end

function panel:onNetworReceive()
end

return panel
