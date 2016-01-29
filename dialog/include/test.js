// send modal information on counselor available hours through web service in dialog_c.aspx

// login modal show on page load
$("#page").ready(function () {
    var title = $("#page-title").val();
    if (title == 'Dialog List') {
        $('table tbody tr').click(function () {
            var that = $(this)
            if (that.hasClass('selected')) { that.removeClass('selected'); $('#participant-id').val('#'); }
            else {
                that.addClass('selected');
                $('#participant-id').val(that.find('td a span').text());
                $('table tbody tr.selected').not(this).removeClass('selected');
            }
            $('#participant-id').trigger('change');
        })
        $('#participant-id').on('change', function () {
            if ($(this).val() != '#') {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/dialog/action.aspx/ParticipantLookup",
                    data:  JSON.stringify({ 'prtid' : $(this).val()}),
                    dataType: 'json',
                    success: function (msg) {
                        if (msg.d == 'failure' || msg.d == 'invalid login') { return; }
                        else {
                            msg = JSON.parse(msg.d);
                            $('ul li a.part-depend').removeClass('hide');
                            $('#recipient-name').val($('#participant-id').val());
                            $('#subject-text').val("Dear " + msg['name_first'] + ", ");
                        }
                    }
                });
            }
            else {
                $('ul li a.part-depend').addClass('hide');
                $('#recipient-name').val('');
                $('#subject-text').val('');
            }
        });
        $('ul li a.part-depend').on('click', function (event) {
            event.preventDefault();
            var part = $('#participant-id');
            if (part.val() == '#') { }
            else { window.location.replace($(this).attr('href') + part.val()); }
        })
    }

    if (title == 'Login') {
        $('#login-modal').modal('show')
        $(".modal-footer").find(".btn.btn-primary[type='submit']").click(function () {
            username = $("#uLogin").val();
            password = $("#uPassword").val();
            unnoticed = $("#unnoticed").is(':checked')
            redirect = $("#from-url").val();
            usertype = $("#user-type").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/dialog/action.aspx/Login",
                data: JSON.stringify({ "ut": usertype, "usr": username, "pwd": password, "rd": redirect, "nn": unnoticed } ),
                dataType: 'json',
                success: function (msg) {
                    msg=JSON.parse(msg.d);
                    alert(msg.redirect +" "+ redirect + " "+ msg.message);
                    if (msg.redirect) {window.location.replace(msg.redirect);}
                }
            });
        });
    }
    else if (title == 'Dialog - Counselor' || title == 'Dialog List') {
        $("#modal1 .modal-footer").find(".btn.btn-primary[type='submit']").click(function () {
            $('#progressouter1').addClass('active');
            $('#progress1').html("Sending...");
            var hours_data = []
            $('#modal1 .modal-body').find("input").each(function () { hours_data.push($(this).val()) })
            var newjson = {'hours': hours_data}
            $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/dialog/action.aspx/setHours",
            data: JSON.stringify(newjson),
            dataType: 'json',
            success: function (msg) {
                $('#progressouter1').removeClass('active');
                $('#progress1').html("Done").delay(2000).html('');
            }
            })
        })
        $('#modal2 .modal-footer').find(".btn.btn-primary[type='submit']").click(function () {
            var recipient = $('recipient-name').val();
            var message = $('message-text').val();
            var subject = $('subject-text').val();
            $('#progressouter2').addClass('active');
            $('#progress2').html("Sending...");
            $.ajax({
                type:"POST",
                contentType: "application/json; charset=utf-8",
                url: "/dialog/action.aspx/sendEmail",
                data: JSON.stringify({ 'prt': recipient, 'sbj': subject, 'msg': message }),
                dateType: 'json',
                success: function (msg) {
                    if (msg.d == 'redirect') { window.location.replace('/dialog/login_alt.aspx?p=counselor'); }
                    else {
                        $('#progressouter2').removeClass('active');
                        $('#progress2').html("Done").delay(200).html('');
                    }
                }
            })
        })
    }
});