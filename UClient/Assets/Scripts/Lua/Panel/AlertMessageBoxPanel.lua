local panel = typesys.def.AlertMessageBoxPanel{
	__super = typesys.BasePanel,
}

function panel:__ctor()
	self._name = "AlertMessageBox"
	self._package = "Alert"
	self._layer = PANEL_LAYER.NOTIFICATION
	self._dependences = nil
	self._immortal = true

	panel.__super.__ctor(self)
end

function panel:_onAwake()
end

function panel:_onDispose()
end

function panel:onShow()
	panel.__super.onShow(self)
end

function panel:onHide()
end

function panel:onEventReceive(id)
end

function panel:onNetworReceive()
end

return panel
