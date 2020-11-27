--[[
Author: fasthro
Date: 2020-11-26 17:51:43
Description: MessageBox-1
--]]
--[[
	使用参数说明

	type = ALERT_MESSAGEBOX_TYPE,
	title = "",
	content = "",
	-- 功能事件
	btn1 = {title = "", cb = function},
	btn2 = {title = "", cb = function},
	btn3 = {title = "", cb = function}, -- 代表X关闭按钮
	-- 关闭事件，存在背景即可响应事件
	btn_close = {title = "", cb = function}
]]
local AlertMessageBoxModule1 =
    typesys.def.AlertMessageBoxModule1 {
    __super = typesys.AlertMessageBoxModule
}

function AlertMessageBoxModule1:_onShow()
    local _background = self._view._background
    local _center = self._view._center
    local _desc = self._desc

    _center._title.text = _desc.title or ""
    _center._content.text = _desc.content or ""

    _center._btn1.onClick:Set(self._ockBtn1, self)
    _center._btn2.onClick:Set(self._ockBtn2, self)
    _center._btn3.onClick:Set(self._ockBtn3, self)
    _background.onClick:Set(self._ockClose, self)

    local _btn = _center._btn
    if _desc.btn1 ~= nil and _desc.btn2 ~= nil then
        _btn:SetSelectedIndex(0)
    elseif _desc.btn1 ~= nil then
        _btn:SetSelectedIndex(1)
    elseif _desc.btn2 ~= nil then
        _btn:SetSelectedIndex(2)
    end

    _center._btn3.visible = _desc.btn_close ~= nil
end

function AlertMessageBoxModule1:_onHide()
end

function AlertMessageBoxModule1:_ockBtn1()
    self._desc.btn1.cb()
    self:hide()
end

function AlertMessageBoxModule1:_ockBtn2()
    self._desc.btn2.cb()
    self:hide()
end

function AlertMessageBoxModule1:_ockBtn3()
    self._desc.btn3.cb()
    self:hide()
end

function AlertMessageBoxModule1:_ockClose()
    if self._desc.btn_close ~= nil and self._desc.btn_close.cb ~= nil then
        self._desc.btn_close.cb()
        self:hide()
    end
end

return AlertMessageBoxModule1
