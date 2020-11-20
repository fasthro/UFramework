
return {
    
    brokecachelen = 5, -- 玩家链接断开之后保持agent多长时间，超过则清楚agent缓存数据, 单位为秒
    
    -- 数据库服务配置
    database = {
        -- 全局命名数据库服务， 这些配置最终是作为参赛传入redis驱动里进行初始化数据库服务
        namespace = {
            host = "127.0.0.1",
            port = 6379,
            db = 0,
            --auth = "passwd",
        },
        
        -- 角色数据库, 有多个
        character = {
            [1] = {
                host = "127.0.0.1",
                port = 6379,
                db = 1,
                --auth = "passwd",
            },
            [2] = {
                host = "127.0.0.1",
                port = 6379,
                db = 2,
                --auth = "passwd",
            },
        },
        
    },
    
    -- 登陆节点启动配置，此节点也作为逻辑上的主节点
    login = {
        -- 登陆服务器网关配置
        conf = {
            name = "login", -- 登陆服务名称， 全局通过cluster能访问到
            host = "0.0.0.0", -- 侦听地址
            port = 8001, -- 侦听端口
            instance = 8, -- 登陆slave验证服务个数
        },
        
        consoleport = 8801, -- 当前节点控制台侦听端口
    },
    
    -- center 节点配置
    center = {
        -- 登陆服务器网关配置
        conf = {
            name = "center", -- 登陆服务名称， 全局通过cluster能访问到
            host = "0.0.0.0", -- 侦听地址
            port = 8002, -- 侦听端口
            instance = 8, -- 登陆slave验证服务个数
        },
        
        consoleport = 8802,
    },
    
    -- node 节点配置
    node1 = {
        -- 登陆服务器网关配置
        conf = {
            name = "node1", -- 登陆服务名称， 全局通过cluster能访问到
            host = "0.0.0.0", -- 侦听地址
            port = 8003, -- 侦听端口
            instance = 8, -- 登陆slave验证服务个数
        },
        
        consoleport = 8803,
    },
    
}

