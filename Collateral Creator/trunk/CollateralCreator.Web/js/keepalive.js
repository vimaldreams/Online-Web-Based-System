
function keep_alive() {
    http_request = new XMLHttpRequest();
    http_request.open('GET', "js/keepalive.js?v=" + (Math.random() * 10000));
    http_request.send(null);
};

setInterval(keep_alive, 300000);  //5 minutes 