    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datacall.aspx.cs" Inherits="cryptodatastore.datacall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>

            <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
                </Triggers>

                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdn_fld" />
                    <asp:Button runat="server" ID="button" style="display: none;" OnClick="button_Click" />
                    <br/>
                </ContentTemplate>
            </asp:UpdatePanel>
             <div id="d1" runat="server">
                
                    <asp:label runat="server" ID="calldata" Text="extracting data from api"></asp:label>
                 <br/>
                 <asp:label runat="server" ID="recieved_data" display="none" Text="recieved data from api"  ></asp:label>
                 <br/>
                 <asp:label runat="server" ID="savingdata" Text="saving data to database"></asp:label>
                 <br/>
                 <asp:label runat="server" ID="saveddata" Text="data saved to database"></asp:label>
                 <br/>
                 <asp:label runat="server" ID="searchusers" Text="searching for users incurring losses"></asp:label>
                 <br/>
                 <asp:label runat="server" ID="sendingmails" Text="sending mails to users incurring losses"></asp:label>
                 <br/>
                 <asp:label runat="server" ID="mailssent"  Text="mails sent"></asp:label>
                    </div>
              
        </div>
    </form>
    <script src="scripts/jquery-3.3.1.min.js"></script>
    <script src="js/owl.carousel.min.js"></script>
    <script src="js/main.js"></script>
    <script type="text/javascript">
        (function update() {
            //setTimeout(update,15000); 
            debugger;
            $.ajax({

                type: 'GET',
                url: 'https://api.nomics.com/v1/currencies/ticker?key=9fe31d6219dd1aa80874159b7a7355c6&ids=BTC,ETH,USDT,MKR,FTXTOKEN,REP,DX,CPX,FSN,IOST,ENG,ENJ,REQ&interval=1d',
                success: function (data) {
                    $.each(data, function (key, value) {

                        $('#' + key).html('$' + value['price']);

                    });

                    var cd = document.getElementById('<%= hdn_fld.ClientID %>');
                    // alert(cd.text)

                    cd.value = JSON.stringify(data);
                    alert(cd.value)
                   
                   
                    var d = document.getElementById('<%= button.ClientID %>');
                    d.click();
                    document.getElementById('calldata').style.display = false
                    document.getElementById('recieved_data').style.display = 'block';
                    //document.getElementById('savingdata').style.display = false
                    //document.getElementById('saveddata').style.display = false
                    //document.getElementById('savingdata').style.display = false
                    //document.getElementById('sendingmails').style.display = false
                    //document.getElementById('mailssent').style.display = false
                   
                   
                },                       // pass existing options
            }).then(function () {           // on completion, restart
               // setTimeout(update, 10000);  // function refers to itself
            });
        })();

    </script>




</body>
</html>
