1、请在log4netConfig.xml配置相应的日志路径
2、请在web.config文件appSettings节点中增加以下配置字段
<appSettings>
<!--自定义token-->
<add key="token" value="token"/>
<!--appID-->
<add key="appID" value="appID"/>
<!--appsecret-->
<add key="appsecret" value="appsecret"/>
</appSettings>