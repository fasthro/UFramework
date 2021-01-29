-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._server = self:GetControllerAt(0)
	self._username = self:GetChildAt(3).asButton
	self._password = self:GetChildAt(4).asButton
	self._local = self:GetChildAt(5).asButton
	self._remote = self:GetChildAt(6).asButton
	self._login = self:GetChildAt(7).asButton
end

function component:setIndex_server(index)
	self._server:SetSelectedIndex(index)
end

function component:setClick_username(callback, target)
	self._username.onClick:Set(callback, target)
end

function component:setClick_password(callback, target)
	self._password.onClick:Set(callback, target)
end

function component:setClick_local(callback, target)
	self._local.onClick:Set(callback, target)
end

function component:setClick_remote(callback, target)
	self._remote.onClick:Set(callback, target)
end

function component:setClick_login(callback, target)
	self._login.onClick:Set(callback, target)
end

fgui.register_extension("ui://u1w4k91lunrx0", component)

return component
