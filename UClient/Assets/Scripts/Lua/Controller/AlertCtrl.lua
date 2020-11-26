--[[
Author: fasthro
Date: 2020-11-26 17:09:34
Description: 提示控制
--]]
local AlertCtrl =
    typesys.def.AlertCtrl {
    __super = typesys.BaseCtrl,
    weak_linePanel = typesys.AlertLinePanel,
    weak_mbPanel = typesys.AlertMessageBoxPanel
}

function AlertCtrl:__ctor()
    AlertCtrl.__super.__ctor(self)
end

function AlertCtrl:__dtor()
end

function AlertCtrl:initialize()
    self.linePanel = PanelManager:showPanel(typesys.AlertLinePanel)
    self.mbPanel = PanelManager:showPanel(typesys.AlertMessageBoxPanel)
end

function AlertCtrl:createLine(content)
    self.linePanel:create(content)
end

return AlertCtrl
