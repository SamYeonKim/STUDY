window.addEventListener(
    'load',
    function() {
        document.body.style.backgroundColor = 'white';
        window.setTimeout(
            function() {
                document.body.style.backgroundColor = '#ABEBC6';
                var msg = document.getElementById("msg");
                msg.textContent = '(NOTE: the background color was changed by sample.js, for checking whether the external js code works)';
            },
            3000);
    });

    function sendMessage() {
        window.cefQuery({
          request: 'BindingTest:' + document.getElementById("message").value,
          onSuccess: function(response) {
            document.getElementById('result').value = 'Response: '+response;
          },
          onFailure: function(error_code, error_message) {}
        });
      }
