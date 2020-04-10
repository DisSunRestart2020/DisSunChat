# DisSunChat公共聊天室
一、运行环境及涉及技术：
------------------
*.Net FrameWork 4.7.2
*EF+MVC5.0
*Code First
*日志log4Net
*Jquery 3.4.1
*Fleck（长连接websocket）
*ToolGood.Words（敏感词过滤算法）
*简单三层结构未涉及IoC（也许后面会用spring.net或castle搞一下）

二、项目开发的背景：
---------------
一开始做这个聊天室的想法很简单，就是花几天时间把之前学习的长连接的知识巩固一下，结果发现做的过程中涉及的东西逐渐增多，光是配置海外服务器的入站规则防火墙就消耗了几天，最后用了大半个月才做完。
其实也不能叫做完，只能说是初步定版V1.0，开始要去忙碌别的事情。而且现在又有了新想法，想在这个基础上，把聊天室做成含有智能聊天机器人的一个玩意，这是后话了。
本来还想用三种方式实现webSocket，也就是Flerk、mosquitto、SignalR，接口类都写好了，结果最后发现，Mosquitto官方提供的客户端不支持C#的websocket，只能暂时放弃。
SignalR呢，虽然能实现websocket，但是不纯粹，看了很多demo对项目的前后端都要变更，暂时没时间就算了。

三、部署&说明：
----------------
1、这个部署应该不难的，先clone本地
2、修改DisSunChat.UI/web.config文件的websocketPath。这个填写websocket内网IP和端口
3、修改DisSunChat.UI/JsCommon/ChatIndex.js，找到websocketInit函数，传入websocket外网服务器IP（如果本地调试可以跟web.config写一样）
4、修改DisSunChat.UI/web.config文件的数据库连接
5、修改DisSunChat.Repos/App.config文件的数据库连接（跟web.config保持一致）
6、数据库使用Code First数据迁移技术就可以还原，亲测有效~
7、用户的唯一性标识identityMd5，目前是通过访问者浏览器型号+手机型号算出来（为什么不用cookie存GuidID？因为微信的cookie不稳定动不动就清除了），然后用端口尾数作为头像序号，所以对方头像第二次上线可能会变化
8、项目界面是仿微信的，但是由于CSS+Div学艺不精只能做成现在这种半吊子了。

四、演示Demo，海外服务器稍微慢点。
----------------
 ![image](http://github.com/DisSunRestart2020/DisSunChat/raw/master/DisSunChat.UI/images/demoUrl.png)

五、下面是一些简单的更新日志
----------------
目前还在艰难码字中...（2020.03.19）

完成了聊天室界面的初步CSS布局（2020.03.20）

添加一个dev分支，用于多点开发同步（2020.03.20）

完成多人聊天功能，且能识别自己和他人，并显示多种头像（2020.03.24）

完成Flerk插件代码的优化，进一步抽象，为后期多插件替换做准备（2020.03.26）

完成前后台的EF分页机制（2020.04.01）

加入敏感词过滤机制（2020.04.03）

成功部署到海外服务器 （2020.04.03）

添加全局错误拦截器和单独的action错误拦截器（2020.04.08）

定版发布《DisSunChat公共聊天室V1.0》（2020.04.10）