
var myVarSe;
var myVarDosSe;

var app = {

	Inicio: function () {
	
		clearInterval(myVarSe);

		myVarSe = setInterval(function () { app.myTimerUltimo() }, 100000);


		//myVarSe = setInterval(function () { app.myTimerUltimo() }, 1000);
	},

	myTimerUltimo: function () {

		document.getElementById("dialogSesionExpirada").style.display = "block";

		myVarDosSe = setInterval(function () { app.myTimerDosUltimo() }, 10000);

		//myVarDosSe = setInterval(function () { app.myTimerDosUltimo() }, 1000);
	},

	Reinicio: function () {

		clearInterval(myVarSe);
		clearInterval(myVarDosSe);

		myVarSe = setInterval(function () { app.myTimerUltimo() }, 100000);

		//myVarSe = setInterval(function () { app.myTimerUltimo() }, 3000);
	},

	ReinicioSesion: function () {
	
		clearInterval(myVarSe);
		clearInterval(myVarDosSe);

		myVarSe = setInterval(function () { app.myTimerUltimo() }, 300000);


		document.getElementById("dialogSesionExpirada").style.display = "none";

		//myVarSe = setInterval(function () { app.myTimerUltimo() }, 3000);
	},

	myTimerDosUltimo: function () {

		app.VerCerrarSesion();
	},

	VerCerrarSesion: function () {
		window.location.href = '/Home/Index/';
	},

};

