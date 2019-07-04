var rsPrintingQueue = [];
var rsPrintingTimerId = null;

function ProcessRSPrintingQueue() {
    if (rsPrintingQueue.length === 0) {
        rsPrintingTimerId = null;
        return;
    }
    var pipeId = RSClient.GetParentWindow().PrintingPipeId;
    var proxy = RSClient.GetProxy();
    var temp = [];
    while (rsPrintingQueue.length > 0) {
        var callback = rsPrintingQueue.shift();
        var result = proxy.ExecScript(pipeId, callback);
        if (!result) {
            temp.push(callback);
        }
    }
    if (temp.length > 0) {
        for (var i = 0; i < temp.length; i++) {
            rsPrintingQueue.push(temp[i]);
        }
    }
    if (rsPrintingQueue.length > 0) {
        rsPrintingTimerId = setTimeout(ProcessRSPrintingQueue, 500);
    } else {
        rsPrintingTimerId = null;
    }
}

var RSClient = {
    OnInit: function () {
        if (!$.browser.msie) {
            var parent = this.GetParentWindow();
            var rsClientPrintContainer = parent.document.getElementById("RSClientPrintContainer");
            if (rsClientPrintContainer === null) {
                var pipeId = "pms_printing_" + (+new Date()) + "_" + Math.floor(Math.random() * (10000 + 1));
                $(parent.document.body).append("<embed id='RSClientPrintContainer' type='application/x-meadco-neptune-ax' pluginspage='http://www.meadroid.com/neptune/download/' width='0' height='0' param-location='" + document.location.origin + _root + "AxPrint/Print?PipeId=" + pipeId + "' />");
                parent.PrintingPipeId = pipeId;
            }
            return;
        }
        $(document.body).append("<div style='display: none'><object id='RSClientPrint' classid='CLSID:0D221D00-A6ED-477C-8A91-41F3B660A832' codebase='" + _root + "ActiveX/RSClientPrint.cab#Version=2005,090,5000,00' viewastext></object></div>");
        this.PrintObject = document.getElementById("RSClientPrint");
        this.ServerPath = "http://" + _serverIP + ":8080/ReportServer";
        this.serverPathAudit = "http://" + _serverIP + ":8080/ReportServer";
        this.PrintReportPath = "%2fPrintCenterChange%2f";
        this.PrintCenterChange = "%2fPrintCenterChange%2f";
        this.AuditReportPath = "%2fHtinns.PMSChangeDB.ReportServer%2f";
        this.printReportPathold = "%2fPrintCenter%2f";
    },
    GetProxy: function () {
        if (this.PrintProxy === undefined || this.PrintProxy === null) {
            var parent = this.GetParentWindow();
            this.PrintProxy = parent.document.getElementById("RSClientPrintProxy");
            if (this.PrintProxy === null) {
                $(parent.document.body).append("<embed id='RSClientPrintProxy' type='application/x-huazhu-npaxplugin' param-object='CLSID:EBFADA35-A9A0-456A-8886-10B9CE3F1BFE' width='0' height='0' />");
                this.PrintProxy = parent.document.getElementById("RSClientPrintProxy");
            }
        }
        return this.PrintProxy;
    },
    GetParentWindow: function () {
        if (this.ParentWindow === undefined || this.ParentWindow === null) {
            this.ParentWindow = window;
            while (this.ParentWindow !== window.parent) {
                this.ParentWindow = window.parent;
            }
        }
        return this.ParentWindow;
    },
    ExecPrintingScript: function (name, args) {
        var callback = name + "(";
        if (args != null) {
            var length = args.length;
            for (var i = 0; i < length; i++) {
                var arg = args[i];
                if (typeof arg === "string") {
                    callback += "'" + arg.replace(/'/g, "\\'") + "'";
                } else {
                    callback += arg;
                }
                if (i !== length - 1) {
                    callback += ",";
                }
            }
        }
        callback += ")";
        var parent = this.GetParentWindow();
        var result = this.GetProxy().ExecScript(parent.PrintingPipeId, callback);
        if (!result) {
            rsPrintingQueue.push(callback);
            if (rsPrintingTimerId === null) {
                rsPrintingTimerId = setTimeout(ProcessRSPrintingQueue, 500);
            }
        }
    },
    Print: function (fileName, reportName) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.Print", arguments);
            return;
        }
        //this.OnInit();
        this.PrintObject.Culture = 1028;
        this.PrintObject.UICulture = 2052;
        if (reportName === "结账单打印") {
            this.PrintObject.MarginTop = 12.7;
            this.PrintObject.MarginBottom = 12.7;
            this.PrintObject.MarginLeft = 2;
            this.PrintObject.MarginRight = 0;

            this.PrintObject.PageHeight = 210;
            this.PrintObject.PageWidth = 148;
        } else {
            this.PrintObject.MarginLeft = 0;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 0;
            this.PrintObject.MarginBottom = 0;

            this.PrintObject.PageHeight = 148;
            this.PrintObject.PageWidth = 210;
        }

        var printReportPath = this.PrintCenterChange;
        var reportPathParameters = printReportPath + fileName + "&rs:ClearSession=true&rs:PersistStreams=False&rs:GetNextStream=False";
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    Print4Parameters: function (fileName, reportName, pHeight, pWidth) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.Print4Parameters", arguments);
            return;
        }
        this.PrintObject.Culture = 1028; //设置默认值 
        this.PrintObject.UICulture = 2052;
        var printReportPath = this.PrintReportPath;
        if (reportName === "杂项单") {
            this.PrintObject.MarginLeft = 12.7;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 12.7;
            this.PrintObject.MarginBottom = 0;
            this.PrintObject.PageHeight = 148;
            this.PrintObject.PageWidth = 210;
            printReportPath = this.PrintCenterChange;
        } else if (reportName === "部分结账单") {
            this.PrintObject.MarginTop = 12.7;
            this.PrintObject.MarginBottom = 12.7;
            this.PrintObject.MarginLeft = 2;
            this.PrintObject.MarginRight = 0;
            this.PrintObject.PageHeight = 210;
            this.PrintObject.PageWidth = 148;
            printReportPath = this.PrintCenterChange;
        } else if (reportName === "历史部分结账单") {
            this.PrintObject.MarginTop = 12.7;
            this.PrintObject.MarginBottom = 12.7;
            this.PrintObject.MarginLeft = 2;
            this.PrintObject.MarginRight = 0;
            this.PrintObject.PageHeight = 210;
            this.PrintObject.PageWidth = 148;
        } else if (reportName === "同意转账单") {
            this.PrintObject.MarginLeft = 5;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 5;
            this.PrintObject.MarginBottom = 0;
            this.PrintObject.PageHeight = 148;
            this.PrintObject.PageWidth = 210;
            printReportPath = this.PrintCenterChange;
        } else if (reportName === "房态实时图") {
            this.PrintObject.MarginLeft = 1;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 1;
            this.PrintObject.MarginBottom = 0;
            this.PrintObject.PageHeight = pHeight;
            this.PrintObject.PageWidth = pWidth;
            printReportPath = this.PrintCenterChange;
        } else if (reportName === "会员卡登记表") {
            this.PrintObject.MarginLeft = 0;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 0;
            this.PrintObject.MarginBottom = 0;
            this.PrintObject.PageHeight = pHeight;
            this.PrintObject.PageWidth = pWidth;
            printReportPath = this.PrintCenterChange;
        } else if (reportName === "预付金凭证") {
            this.PrintObject.MarginLeft = 2;
            this.PrintObject.MarginTop = 0;
            this.PrintObject.MarginRight = 0;
            this.PrintObject.MarginBottom = 0;
            this.PrintObject.PageHeight = 148;
            this.PrintObject.PageWidth = 210;
            printReportPath = this.PrintCenterChange;
        } else {
            this.PrintObject.MarginLeft = 12.7;
            this.PrintObject.MarginTop = 12.7;
            this.PrintObject.MarginRight = 12.7;
            this.PrintObject.MarginBottom = 12.7;
            this.PrintObject.PageHeight = pHeight;
            this.PrintObject.PageWidth = pWidth;
            printReportPath = this.PrintCenterChange;
        }

        var reportPathParameters = printReportPath + fileName + "&rs:ClearSession=true&rs:PersistStreams=False&rs:GetNextStream=False";
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    Print4ParametersNew: function (fileName, reportName) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.Print4ParametersNew", arguments);
            return;
        }

        this.PrintObject.Culture = 1028; //设置默认值 
        this.PrintObject.UICulture = 2052;
        this.PrintObject.MarginLeft = 0;
        this.PrintObject.MarginTop = 0;
        this.PrintObject.MarginRight = 0;
        this.PrintObject.MarginBottom = 0;
        this.PrintObject.PageHeight = 210;
        this.PrintObject.PageWidth = 80;
        var printReportPath = this.PrintCenterChange;
        var reportPathParameters = printReportPath + fileName + "&rs:ClearSession=true&rs:PersistStreams=False&rs:GetNextStream=False";
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    PrintCN: function (fileName, reportName, mLeft, mTop, mRight, mBottom, pHeight, pWidth) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.PrintCN", arguments);
            return;
        }
        //this.OnInit();
        var isMini = (fileName.indexOf("Mini") !== -1);
        if (isMini) {
            mLeft = 0;
            mTop = 0;
            mRight = 0;
            mBottom = 0;
        }
        this.PrintObject.Culture = 1028;
        this.PrintObject.UICulture = 2052;
        this.PrintObject.MarginLeft = mLeft;
        this.PrintObject.MarginTop = mTop;
        this.PrintObject.MarginRight = mRight;
        this.PrintObject.MarginBottom = mBottom;
        if (reportName === "预付金凭证" || reportName === "临时住宿登记单" || reportName === "临时住宿登记单&预付金凭证") {
            if (isMini) {
                this.PrintObject.PageHeight = 210;
                this.PrintObject.PageWidth = 80;
            } else {
                this.PrintObject.PageHeight = 148;
                this.PrintObject.PageWidth = 210;
            }
        } else {
            this.PrintObject.PageHeight = pHeight;
            this.PrintObject.PageWidth = pWidth;
        }

        var reportPathParameters = this.PrintReportPath + fileName;
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    PrintIN: function (fileName, reportName, rsCulture, rsUICulture, mLeft, mTop, mRight, mBottom, pHeight, pWidth, bAuthen) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.PrintIN", arguments);
            return;
        }
        //this.OnInit();
        this.PrintObject.Culture = rsCulture;
        this.PrintObject.UICulture = rsUICulture;
        this.PrintObject.MarginLeft = mLeft;
        this.PrintObject.MarginTop = mTop;
        this.PrintObject.MarginRight = mRight;
        this.PrintObject.MarginBottom = mBottom;
        this.PrintObject.PageHeight = pHeight;
        this.PrintObject.PageWidth = pWidth;
        this.PrintObject.Authenticate = bAuthen;
        var reportPathParameters = this.PrintReportPath + fileName + "&rs:ClearSession=true&rs:PersistStreams=False&rs:GetNextStream=False";
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    PrintAuditing: function (fileName, reportName) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.PrintAuditing", arguments);
            return;
        }
        this.PrintObject.Culture = 1028;
        this.PrintObject.UICulture = 2052;
        this.PrintObject.MarginLeft = 1;
        this.PrintObject.MarginTop = 1;
        this.PrintObject.MarginRight = 1;
        this.PrintObject.MarginBottom = 1;
        this.PrintObject.PageHeight = 210;
        this.PrintObject.PageWidth = 297;
        var reportPathParameters = this.AuditReportPath + fileName;
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    },
    PrintCNOld: function (fileName, reportName, mLeft, mTop, mRight, mBottom, pHeight, pWidth) {
        if (!$.browser.msie) {
            this.ExecPrintingScript("RSClient.PrintCNOld", arguments);
            return;
        }
        this.PrintObject.Culture = 1028;
        this.PrintObject.UICulture = 2052;
        this.PrintObject.MarginLeft = mLeft;
        this.PrintObject.MarginTop = mTop;
        this.PrintObject.MarginRight = mRight;
        this.PrintObject.MarginBottom = mBottom;
        if (reportName === "预付金凭证" || reportName === "临时住宿登记单" || reportName === "临时住宿登记单&预付金凭证") {
            this.PrintObject.PageHeight = 148;
            this.PrintObject.PageWidth = 210;
        } else {
            this.PrintObject.PageHeight = pHeight;
            this.PrintObject.PageWidth = pWidth;
        }
        var reportPathParameters = this.printReportPathold + fileName;
        this.PrintObject.Print(this.ServerPath, reportPathParameters, reportName);
    }
}
$(function () {
    RSClient.OnInit();
});


////////var serverPath = "http://10.1.254.5/ReportServer";    //正式的ReportServer报表服务器
//var serverPath = "http://10.1.249.65:7747/ReportServer"; //测试的ReportServer报表服务器
////var  serverPath = "http://192.168.255.107/ReportServer"; //培训的ReportServer报表服务器
//var printReportPath = "%2fPrintCenterChange%2f";
//MarginLeft Double RW 报表设置 获取或设置左边距。如果开发人员没有设置或报表中未指定，则默认值为 12.2 毫米。

//MarginRight Double RW 报表设置 获取或设置右边距。如果开发人员没有设置或报表中未指定，则默认值为 12.2 毫米。

//MarginTop Double RW 报表设置 获取或设置上边距。如果开发人员没有设置或报表中未指定，则默认值为 12.2 毫米。

//MarginBottom Double RW 报表设置 获取或设置下边距。如果开发人员没有设置或报表中未指定，则默认值为 12.2 毫米。

//PageWidth Double RW 报表设置 获取或设置页宽。如果开发人员或报表定义中未进行设置，则默认值为 215.9 毫米。

//PageHeight Double RW 报表设置 获取或设置页高。如果开发人员或报表定义未对其进行设置，则默认值为 279.4 毫米。

//Culture Int32 RW 浏览器区域设置 指定区域设置标识符 (LCID)。此值将确定用户输入的度量单位。例如，如果用户键入 3，
//则在语言为法语时，值将按毫米度量；在语言为英语（美国）时，值将按英寸度量。
//有效值包括：1028、1031、1033、1036、1040、1041、1042、2052、3082。

//UICulture String RW 客户端区域性 指定对话框字符串的本地化语言。“打印”对话框中的文本已本地化为以下语言：
//简体中文、繁体中文、英语、法语、德语、意大利语、日语、朝鲜语和西班牙语。
//有效值包括：1028、1031、1033、1036、1040、1041、1042、2052、3082。

//Authenticate Boolean RW False 指定控件是否向报表服务器发出 GET 命令，以启动无会话打印连接。

//*/


///*

//以上数据是针对A4纸张打印的。
//下面是常见的A5系列规格：
//A4（16k）297mm×210mm； 
//A5（32k）210mm× 148mm； 
//A6（64k）144mm×105mm； 
//A3（8k）420mm×297mm； 
//注意：其中A3（8k）尺寸尚未定入，但普遍用。

// Web PMS部分打印是A5纸张，需要注意这一点。
//*/
