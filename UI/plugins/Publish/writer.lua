--[[
Author: fasthro
Date: 2020-09-24 15:18:37
Description: code writer
--]]

local writer = fclass()

function writer:ctor()
    self:reset()
end

function writer:reset()
    self._seek = 0
    self._textlines = {}
end

function writer:writeln(content)
    self._seek = self._seek + 1
    if content == nil then
        content = "\n"
    else
        content = content .. "\n"
    end
    self._textlines[self._seek] = content
end

function writer:writelnTab(content, layer)
    if layer == nil then
        layer = 1
    end
    local prefix = ""
    for k = 1, layer do
        prefix = prefix .. "\t"
    end
    self:writeln(prefix .. content)
end

function writer:tostring()
    local content = ""
    for i=1, self._seek do
        content = content .. self._textlines[i]
    end
    return content
end

function writer:save(path)
    utils.fileCreateText(path, self:tostring())
end

CodeFileWriter = writer