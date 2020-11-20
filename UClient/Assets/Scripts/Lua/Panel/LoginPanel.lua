local panel =
    typesys.def.LoginPanel {
    __super = typesys.BasePanel,
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

    local on_click_login = function()
        LoginCtrl:login()
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
