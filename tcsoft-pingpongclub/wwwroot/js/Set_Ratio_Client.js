"use strict";
var conn = new signalR.HubConnectionBuilder().withUrl("/setRatio").build();
const id = new URLSearchParams(window.location.search).get("id");

conn.on("GetRatio", (idMatch, ratio1, ratio2, ratio3) => {
    if (idMatch == id) {
        const point1 = ratio1.split(" - ");
        const point2 = ratio2.split(" - ");
        const point3 = ratio3.split(" - ");
        var pts1 = 0, pts2 = 0;

        if (point1[0] > point1[1] && point1[0] >= 21) {
            pts1++;
        } else if (point1[0] < point1[1] && point1[1] >= 21) {
            pts2++;
        }

        if (point2[0] > point2[1] && point2[0] >= 21) {
            pts1++;
        } else if (point2[0] < point2[1] && point2[1] >= 21) {
            pts2++;
        }
        if (point3[0] > point3[1] && point3[0] >= 21) {
            pts1++;
        } else if (point3[0] < point3[1] && point3[1] >= 21) {
            pts2++;
        }
        document.getElementById("ratio1").textContent = ratio1;

        document.getElementById("ratio2").textContent = ratio2;

        document.getElementById("ratio3").textContent = ratio3;
        document.getElementById("pts").textContent = pts1 + " - " + pts2;
    }


})
conn.start().then(() => {
    console.log("hihihihi")
}).catch(function (err) {
    return console.log(err.toString());
});

//document.getElementByid("sentRatio").addeventlistener("click",(event) => {
//    var idmatch = document.getelementbyid("sentRatio").value;
//    var point1 = document.getelementbyid("point1").value;
//    var point2 = document.getelementbyid("point2").value;
//    var point3 = document.getelementbyid("point3").value;
//    var point4 = document.getelementbyid("point4").value;
//    var point5 = document.getelementbyid("point5").value;
//    var point6 = document.getelementbyid("point6").value;
//    var ratio1 = point1 + " - " + point2;
//    var ratio2 = point3 + " - " + point4;
//    var ratio3 = point5 + " - " + point6;
//    conn.invoke("sendratio", idmatch, ratio1, ratio2, ratio3);
//    event.preventdefault();
//})
