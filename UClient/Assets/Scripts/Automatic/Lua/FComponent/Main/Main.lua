-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._fps = self:GetChildAt(2).asTextField
	self._delay = self:GetChildAt(4).asTextField
	self._ping = self:GetChildAt(6).asTextField
	self._moveJoy = self:GetChildAt(8).asCom
end

function component:setText_fps(text)
	self._fps.text = text
end

function component:setText_delay(text)
	self._delay.text = text
end

function component:setText_ping(text)
	self._ping.text = text
end

fgui.register_extension("ui://slaudw4wz4a20", component)

return component
