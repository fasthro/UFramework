--[[
Author: fasthro
Date: 2020-11-26 17:09:34
Description: 提示控制
--]]
local AlertCtrl =
    typesys.def.AlertCtrl {
    __super = typesys.BaseCtrl,
    weak_linePanel = typesys.AlertLinePanel,
    weak_messageBoxPanel = typesys.AlertMessageBoxPanel
}

function AlertCtrl:__ctor()
    AlertCtrl.__super.__ctor(self)
end

function AlertCtrl:__dtor()
end

function AlertCtrl:initialize()
    self.linePanel = PanelManager:showPanel(typesys.AlertLinePanel)
    self.messageBoxPanel = PanelManager:showPanel(typesys.AlertMessageBoxPanel)
end

-- 创建Line提示
-- @param contents 内容列表
-- @param intervalTime 每条出现内容间隔时间
function AlertCtrl:createLine(content)
    self.linePanel:createLine(content)
end

-- 创建Line提示
-- @param content 内容
function AlertCtrl:createLines(contents, intervalTime)
    self.linePanel:createLines(contents, intervalTime)
end

-- 创建MessageBox提示
-- @param desc 描述
function AlertCtrl:createMessageBox(desc)
    --[[
        MessageBox1 创建描述

        AlertCtrl:createMessageBox(
        {
            type = ALERT_MESSAGEBOX_TYPE.NORMAL,
            title = "测试标题",
            content = "测试内容",
            btn1 = {
                title = "按钮1标题",
                cb = function()
                    -- 按钮1回调方法
                end
            },
            btn2 = {
                title = "按钮2标题",
                cb = function()
                    -- 按钮2回调方法
                end
            },
            btn3 = {
                title = "按钮3标题",
                cb = function()
                    -- 按钮3回调方法
                end
            },
            btn_close = {
                cb = function()
                    -- 背景点击回调
                end
            }
        }
    )
    ]]
    self.messageBoxPanel:create(desc)
end

return AlertCtrl
