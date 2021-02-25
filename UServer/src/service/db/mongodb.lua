local skynet = require "skynet"
require "skynet.manager"
local db = require "mongodblogic"

skynet.start(function()
	skynet.dispatch("lua", function(session, address, cmd, ...)
		local f = db[cmd]
		if f then
			skynet.ret(skynet.pack(f(...)))
		else
			error(string.format("Unknown command %s", tostring(cmd)))
		end
	end)

	skynet.register ".mongodb"
end)
