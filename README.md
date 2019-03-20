# Pons
## 实现类似于键盘宏功能

* 1 key.1=Key.F1+Key.1
* 2 循环播放某些键
* 3 集成了鼠标连点器功能


### 底层两种实现方式：
* 1 Windows键盘事件消息，将来自真实键盘的参数设置为True(之前在九阴真经这款游戏上可以用的，后来再试的不能用了，就没管为什么不能用了)
* 2 调用DD模块


### 举例说明：实现模拟先切护腕再按招式的过程
#### 按下1，实现先按下F1，再按1，1=F1+1
#### 按F1启动，默认ESC结束

* 点击+号,列表会出现新一项
![image](https://github.com/tiancai4652/Pons/blob/master/Images/1.png)
* 鼠标点击None,按下自己想启动的键,本例子启动键F1，此时，None会自动变成F1，点击编辑按钮
![image](https://github.com/tiancai4652/Pons/blob/master/Images/2.png)
* 左边按下主键， Keys.1
* 右边输入映射键， Keys.F1，Kyes.1
![image](https://github.com/tiancai4652/Pons/blob/master/Images/3.png)
![image](https://github.com/tiancai4652/Pons/blob/master/Images/4.png)
* '》》'输入  ‘《《’删除

* 回到主界面，点击三角形执行
![image](https://github.com/tiancai4652/Pons/blob/master/Images/5.png)



## 鼠标连点器功能

![image](https://github.com/tiancai4652/Pons/blob/master/Images/6.png)
* 弹出连点器程序，设置一个热键，点击启用
![image](https://github.com/tiancai4652/Pons/blob/master/Images/7.png)
* 按下设置的热键，启动连点，再按一下，取消连点
