--[[
Author: fasthro
Date: 2020-11-11 17:19:01
Description: 登录控制
--]]
local LoginCtrl =
    typesys.def.LoginCtrl {
    __super = typesys.BaseCtrl
}

function LoginCtrl:__ctor()
    LoginCtrl.__super.__ctor(self)
end

function LoginCtrl:__dtor()
end

function LoginCtrl:initialize()
    _managerCenter:getManager(typesys.PanelManager):showPanel(typesys.LoginPanel)
end

function LoginCtrl:login()
    print("login")
end

return LoginCtrl
