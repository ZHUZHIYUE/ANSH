using System;
/// <summary>
/// 异常消息
/// </summary>
public class ANSHExceptions : Exception {
    /// <summary>
    /// 错误代码
    /// </summary>
    public int ERRORCODE {
        get;
        set;
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string ERRORMSG {
        get;
        set;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="error">错误类型</param>
    public ANSHExceptions (ANSHErrorCodes error) : base (error.ToString ()) {
        ERRORCODE = (int) error;
        ERRORMSG = error.ToString ();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="error">错误类型</param>
    /// <param name="errorMsg">错误信息</param>
    public ANSHExceptions (ANSHErrorCodes error, string errorMsg) : base (errorMsg) {
        ERRORCODE = (int) error;
        ERRORMSG = errorMsg;
    }
}
/// <summary>
/// 错误类型
/// </summary>
public enum ANSHErrorCodes {

    /// <summary>
    /// OAuth验证中的错误类型
    /// </summary>
    无效的身份令牌 = 10001,

    /// <summary>
    /// OAuth验证中的错误类型
    /// </summary>
    未能识别的AppID和AppSecret组合 = 10002,

    /// <summary>
    /// 一般指该操作权限不足
    /// </summary>
    权限不足受限 = 20001,

    /// <summary>
    /// 一般指访问次数超过上限
    /// </summary>
    访问次数受限 = 20002,

    /// <summary>
    /// 一般指访问超时
    /// </summary>
    访问超时 = 30000,

    /// <summary>
    /// 对象不存在，一般用于修改操作
    /// </summary>
    不存在的对象 = 40001,

    /// <summary>
    /// 对象已存在，一般用于添加操作
    /// </summary>
    已存在的对象 = 40002,

    /// <summary>
    /// 一般指提交的数据无效
    /// </summary>
    无效的数据 = 40003,

    /// <summary>
    /// 所有未定义的错误类型
    /// </summary>
    服务器内部错误 = 50000,
}