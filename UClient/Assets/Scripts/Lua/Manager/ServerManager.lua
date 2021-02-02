--------------------------------------------------------------------------------
-- @Author: fasthro
-- @Date: 2021/01/29 12:39
-- @Description: 服务器管理
--------------------------------------------------------------------------------

local ServerManager =
    typesys.def.ServerManager {
    __super = typesys.BaseManager,
    _use_local = false,
    serverid = "gate_name1",
    login_server_ip = "",
    login_server_port = 0,
    game_server_ip = "",
    game_server_port = 0,
    battle_server_ip = "",
    battle_server_port = 0
}

function ServerManager:initialize()
    if self._use_local then
        self:useLocalServer()
    else
        self:useRemoteServer()
    end
end

function ServerManager:useLocalServer()
    self._use_local = true

    self.login_server_ip = "192.168.1.171"
    self.login_server_port = 8001

    self.game_server_ip = "192.168.1.171"
    self.game_server_port = 9001

    self.battle_server_ip = "127.0.0.1"
    self.battle_server_port = 15940
end

function ServerManager:useRemoteServer()
    self._use_local = false

    self.login_server_ip = "39.97.236.132"
    self.login_server_port = 8001

    self.game_server_ip = "39.97.236.132"
    self.game_server_port = 9001

    self.battle_server_ip = "127.0.0.1"
    self.battle_server_port = 15940
end

function ServerManager:isUseLocal()
    return self._use_local
end

return ServerManager
