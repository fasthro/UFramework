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

function panel:onAwake()
end

function panel:onDispose()
end

function panel:onShow()
    panel.__super.onShow(self)

    self.view._state:SetSelectedIndex(0)

    self.view._username._input.text = "fasthro"
    self.view._password._input.text = "password"

    -- 登录按钮
    local on_click_login = function()
        local username = self.view._username._input.text
        local password = self.view._password._input.text
        local serverid = "gate_name1"
        LoginCtrl:connectLoginServer("192.168.1.171", 8001, username, password, serverid)
    end
    self:_bindClick(self.view._login, on_click_login)

    -- touch start game
    local on_click_touch = function()
        LoginCtrl:connectGameServer("192.168.1.171", 9001)
    end
    self:_bindClick(self.view._touch, on_click_touch)

    -- register login succeed
    EventManager:once(EVENT_NAMES.LOGIN_SUCCEED, self.onLoginSucceed, self)
end

function panel:onHide()
    EventManager:remove(EVENT_NAMES.LOGIN_SUCCEED, self.onLoginSucceed)
end

function panel:onNetReceived(pack)
end

function panel:onLoginSucceed()
    self.view._state:SetSelectedIndex(1)
end

return panel
