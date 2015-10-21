using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Prueba : Form
    {
        int contadorCifras = 0, tick = 0;
        Double [] operandos = new Double[2] {0,0};
        Double resultado1 = 0, memoria = 0;
        char operacion;
        String buffer = "";
        bool primeraOperacion = true, encadenado = false, nuevoNumero = true, especial = false, operacionErronea = false;

        public Prueba()
        {
            InitializeComponent();
        }

        private void introducirNumero(Double numero)
        {
            if(numero == 0d && tResultado.Text == "0")
                return;
            if (nuevoNumero == true)
            {
                tResultado.Clear();
                nuevoNumero = false;
            }
            if (contadorCifras < 16)
            {
                if (tResultado.Text == "0")
                    tResultado.Text = "";
                tResultado.Text += numero.ToString();
                buffer += numero;
                operandos[tick] = Double.Parse(buffer);
                contadorCifras++;
            }
        }

        private void introducirSigno(char signo)
        {
            if (especial == false)
            {
                if (encadenado == false)
                    tHistorial.Text = operandos[0].ToString();
                else
                 tHistorial.Text += buffer;
            }
            else
                especial = false;
            tHistorial.Text += " " + signo + " ";
            buffer = "";
            contadorCifras = 0;
            nuevoNumero = true;
            if (encadenado == false)
            {
                tick = 1;
                encadenado = true;
                primeraOperacion = false;
                tResultado.Clear();
            }
            else
            {
                operar();
                tResultado.Text = resultado1.ToString();
                primeraOperacion = false;
            }
            operacion = signo;
        }

        private void operar()
        {
            if (primeraOperacion == false)
            {
                if (operacion == '+')
                    resultado1 = operandos[0] + operandos[1];
                else if (operacion == '-')
                    resultado1 = operandos[0] - operandos[1];
                else if (operacion == '*')
                    resultado1 = operandos[0] * operandos[1];
                else if (operacion == '/')
                    resultado1 = operandos[0] / operandos[1];
                operandos[0] = resultado1;
            }
            tick = 1;
        }

        private void igual()
        {
            if (operacionErronea == true)
                return;
            if (tick == 1)
            {
                operar();
                tResultado.Text = resultado1.ToString();
                tHistorial.Clear();
                encadenado = false;
                operandos[1] = 0;
                tick = 0;
            }
        }
        private void teclado(String entrada)
        {
            if (operacionErronea == true)
                return;
            try
            {
                introducirNumero(Double.Parse(entrada));
            }
            catch(FormatException)
            {
                char op = entrada[0];
                if (op == '=' || entrada == "\r")
                    igual();
                else if (op == '.' || op == ',')
                {
                    buffer += ",";
                    tResultado.Text += ",";
                }
                else if (entrada == "+" || entrada == "-" || entrada == "*" || entrada == "*")
                    introducirSigno(op);
                else
                    introducirOperacionEspecial(entrada);   
            }
        }
        private void introducirOperacionEspecial(String entrada)
        {
            if (operacionErronea == true)
                return;
            if (entrada == "√")
                raiz();
            else if (entrada == "%")
                porcentaje();
            else if (entrada == "1/x")
                inverso();
            else if (entrada == "±")
                cambioSigno();
            else if (entrada == "C")
                borrarTodo();
            else if (entrada == "CE")
                borrarValor();
            else if (entrada == "M+")
                memoriaMas();
            else if (entrada == "M-")
                memoriaMenos();
            else if (entrada == "MR")
                memoriaRestaurar();
            else if (entrada == "MS")
                memoriaGuardar();
            else if (entrada == "MC")
                memoriaGuardar();
            buttonigual.Focus();
        }
        private void borrarTodo()
        {
            operandos[0] = 0;
            operandos[1] = 0;
            tHistorial.Text = "";
            tResultado.Text = "0";
            contadorCifras = 0;
            resultado1 = 0;
            tick = 0;
            buffer = "";
            operacion = ' ';
            primeraOperacion = true;
            encadenado = false;
            buttonigual.Focus();
        }
        private void pulsacionRaton(object sender, EventArgs e)
        {
            if (operacionErronea == true)
                return;
            Button boton = (Button) sender;
            teclado(boton.Text);
            buttonigual.Focus();
        }
        private void pulsacionTeclado(object sender, KeyPressEventArgs e)
        {
            if (operacionErronea == true)
                return;
            teclado(e.KeyChar.ToString());
        }

        private void borrarAnterior()
        {
            if (operacionErronea == true)
                return;
            if (buffer.Length > 1)
            {
                try
                {
                    buffer = buffer.Substring(0, buffer.Length - 1);
                }
                catch
                {
                    return;
                }
                tResultado.Text = tResultado.Text.Substring(0, tResultado.Text.Length - 1);
                operandos[tick] = Double.Parse(buffer);
                contadorCifras--;
                buttonigual.Focus();
            }
            else
            {
                buffer = "0";
                tResultado.Text = "0";
                operandos[tick] = Double.Parse(buffer);
                buttonigual.Focus();
            }
        }
        private void inverso()
        {
            especial = true;
            tHistorial.Text += "reciproc(" + operandos[tick] + ")";
            operandos[tick] = 1 / operandos[tick];
            tResultado.Text = operandos[tick].ToString();
        }
        private void cambioSigno()
        {
            especial = true;
            operandos[tick] = -operandos[tick];
            tResultado.Text = operandos[tick].ToString();
        }
        private void borrarValor()
        {
            operandos[tick] = 0;
            tResultado.Text = "0";
        }
        private void memoriaMas()
        {
            checkMemoria.Checked = true;
            memoria += operandos[tick];
        }
        private void memoriaMenos()
        {
            checkMemoria.Checked = true;
            memoria -= operandos[tick];
        }
        private void memoriaBorrar()
        {
            checkMemoria.Checked = false;
            memoria = 0;
        }
        private void memoriaRestaurar()
        {
            operandos[tick] = memoria;
            tResultado.Text = operandos[tick].ToString();
        }
        private void memoriaGuardar()
        {
            checkMemoria.Checked = true;
            memoria = operandos[tick];
        }
        private void raiz()
        {
            especial = true;
            tHistorial.Text += "sqrt(" + operandos[tick] + ")";
            operandos[tick] = Math.Sqrt(operandos[tick]);
            tResultado.Text = operandos[tick].ToString();
        }
        private void porcentaje()
        {
            if (tick == 1)
            {
                especial = true;
                operandos[1] = operandos[0] / 100 * operandos[1];
                tResultado.Text = operandos[1].ToString();
            }
        }
    }
}