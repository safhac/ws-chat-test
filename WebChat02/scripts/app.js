    $(document).ready(function () {

        $('#msgContainer').hide();

    refreshUserList();
                setInterval(refreshUserList, 10000);


                // login
                $('#login').click(function () {
                    if (!$('#username').val() == '') {
                        var username = $('#username').val();

                        $.ajax({
        url: 'http://localhost:53537/ChatService.asmx/Login',
                            contentType: 'application/json;charset=utf-8',
                            data: '{username:' + JSON.stringify(username) + '}',
                            method: 'post',
                            success: function (data) {
        $('#messages').append(data);
    $('#msgContainer').show();
                                $('#loginContainer').hide();
                                setInterval(getMessages, 10000);

                            },
                            error: function (err) {

        console.log('error ' + err.responseText);
    },


                        });
                    }
                });

                // send message
                $('#send').click(function () {
                    if (!$('#message').val() == '') {
                        var Msg =
                            {
        "user": {"name": $('#username').val() },
                                "msg": $('#message').val(),
                                "time": ""
                            }
                        $.ajax({
        url: 'http://localhost:53537/ChatService.asmx/SendMsg',
                            dataType: 'json',
                            contentType: 'application/json;charset=utf-8',
                            data: '{"incoming" :' + JSON.stringify(Msg) + '}',
                            method: 'post',
                            success: function () {
        getMessages();
    $('#message').val('')
                            },
                            error: function (err) {

        console.log('error ' + err.responseText);
    }

                        });
                    }
                });

                // get users task
                function refreshUserList() {
        $.ajax({
            url: 'http://localhost:53537/ChatService.asmx/GetOnlineUsers',
            contentType: 'application/json;charset=utf-8',
            method: 'get',
            success: function (data) {
                var userListHtml = '<ul>'
                $(data).each(function (index, user) {

                    userListHtml += '<li><a href="since ' + user.logintime + '">' + user.name + '</a></li>';
                })

                $('#userList').html(userListHtml + '</ul>');

            },
            error: function (err) {

                console.log('error ' + err.responseText);
            }

        });
    }

                // get messages task
                function getMessages() {
                    var username = $('#username').val();
                    $.ajax({
        url: 'http://localhost:53537/ChatService.asmx/GetMessages',
                        contentType: 'application/json;charset=utf-8',
                        method: 'post',
                        data: '{username:' + JSON.stringify(username) + '}',
                        success: function (data) {
                            var msgListHtml = '';
                            $(data).each(function (index, msg) {

        msgListHtml += '<p>' + msg.time + ' ' + msg.user.name + ' : ' + msg.msg + '</p>';
    })

                            $('#messages').html(msgListHtml);

                        },
                        error: function (err) {

        console.log('error ' + err.responseText);
    }

                    });
                }
            });
