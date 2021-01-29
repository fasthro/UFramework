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
    
    if ServerManager:isUseLocal() then
        self.view._server:SetSelectedIndex(0)
    else
        self.view._server:SetSelectedIndex(1)
    end

    self.view._username._input.text = "fasthro"
    self.view._password._input.text = "password"

    -- 本地服务器
    local on_click_local = function()
        ServerManager:useLocalServer()
    end
    self:_bindClick(self.view._local, on_click_local)

    -- 远程服务器
    local on_click_remote = function()
        ServerManager:useRemoteServer()
    end
    self:_bindClick(self.view._remote, on_click_remote)

    -- 登录按钮
    local on_click_login = function()
        local username = self.view._username._input.text
        local password = self.view._password._input.text

        LoginCtrl:connectLoginServer(ServerManager.login_server_ip, ServerManager.login_server_port, username, password, ServerManager.serverid)
    end
    self:_bindClick(self.view._login, on_click_login)

    -- register login succeed
    EventManager:once(EVENT_NAMES.LOGIN_SERVER_AUTH_SUCCEED, self.onLoginServerAuthSucceed, self)
    EventManager:once(EVENT_NAMES.GAME_SERVER_AUTH_SUCCEED, self.onGameServerAuthSucceed, self)
end

function panel:onHide()
    EventManager:remove(EVENT_NAMES.LOGIN_SERVER_AUTH_SUCCEED, self.onLoginServerAuthSucceed)
    EventManager:remove(EVENT_NAMES.LOGIN_SERVER_AUTH_SUCCEED, self.onGameServerAuthSucceed)
end

function panel:onNetReceived(cmd, pack)
end

function panel:onLoginServerAuthSucceed()
    LoginCtrl:connectGameServer(ServerManager.game_server_ip, ServerManager.game_server_port)
end

function panel:onGameServerAuthSucceed()
    self:hide()
    PanelManager:showPanel(typesys.MainPanel)
end

return panel
