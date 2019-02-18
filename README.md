# Asp.Net Standard Helper (ANSH)  

[![badge language](https://img.shields.io/badge/language-C%23-green.svg)](https://github.com/ZHUZHIYUE/ANSH)
[![badge test](https://img.shields.io/badge/test-xunit.net-green.svg)](https://docs.microsoft.com/zh-cn/dotnet/core/testing/unit-testing-with-dotnet-test?view=aspnetcore-2.1)
[![GitHub license](https://img.shields.io/github/license/ZHUZHIYUE/ANSH.svg)](https://github.com/ZHUZHIYUE/ANSH/blob/master/LICENSE)
[![Build Status](https://www.travis-ci.org/ZHUZHIYUE/ANSH.svg?branch=master)](https://www.travis-ci.org/ZHUZHIYUE/ANSH)
[![GitHub issues](https://img.shields.io/github/issues/ZHUZHIYUE/ANSH.svg)](https://github.com/ZHUZHIYUE/ANSH/issues)
[![GitHub forks](https://img.shields.io/github/forks/ZHUZHIYUE/ANSH.svg)](https://github.com/ZHUZHIYUE/ANSH/network)
[![GitHub stars](https://img.shields.io/github/stars/ZHUZHIYUE/ANSH.svg)](https://github.com/ZHUZHIYUE/ANSH/stargazers)  
解决方案基于[NET Standard 2.0](https://docs.microsoft.com/zh-cn/dotnet/standard/net-standard)开发，主要是将一些项目开发中常用的方法进行整合。  

## 准备工作  

**确保您的操作系统中已经安装[NuGet](/docs/NuGet.md)**  
在命令行中运行

```bash
#!/bin/bash
$ cd ${ANSH.sln}
$ nuget restore
```  

## 类库  

* **ANSH.ALL**  
ANSH所有类库引用
* **ANSH.API**  
WebApi SDK操作类库，请求-响应  
* **ANSH.OAuth**  
OAuth2授权操作类库
* **ANSH.AspNetCore**  
AspNetCore操作的类库
* **ANSH.Common**  
通用类库，不需要第三方package且没有创建单独类库的方法都集成在此类库中
* **ANSH.DataBase.Connection**  
数据库连接类库，包括对事物的处理
* **ANSH.DataBase.EFCore**  
基于EFCore使用的基类
* **ANSH.DataBase.EFCore.MySQL**  
EFCore对MySql进行操作的类库
* **ANSH.DataBase.EFCore.SQLServer**  
EFCore对SqlServer进行操作的类库
* **ANSH.Json**  
Json操作类库
* **Testing.Server**  
[XUnit.Net](https://docs.microsoft.com/zh-cn/dotnet/core/testing/unit-testing-with-dotnet-test?view=aspnetcore-2.1) 测试
* **Testing.UI**  
测试DEMO