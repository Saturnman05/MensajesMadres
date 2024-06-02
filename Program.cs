using MensajeDiaMadres.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MensajeDiaMadres.DataSet1;

namespace MensajeDiaMadres
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            SqlTransaction transaction = null;
            PrincipalTableAdapter tblPrincipal = new PrincipalTableAdapter();
            DetalleTableAdapter tblDetalle = new DetalleTableAdapter();

            Console.Write("Opciones:\n1.Crear Principal\n2.Enviar mensaje\nOpcion: ");
            int opcion = Int32.Parse(Console.ReadLine());

            int codigo, relacion;
            string descripcion, mensajeCorto, mensaje, enviadoPor, para;

            try
            {
                switch (opcion)
                {
                    case 1:
                        tblPrincipal.Connection.Open();
                        transaction = tblPrincipal.Connection.BeginTransaction();
                        tblPrincipal.Transaction = transaction;

                        Console.Write("Codigo: ");
                        codigo = Int32.Parse(Console.ReadLine());

                        Console.Write("Descripcion: ");
                        descripcion = Console.ReadLine();

                        tblPrincipal.ppUpsertPrincipal(codigo, descripcion);
                        transaction.Commit();
                        break;
                    case 2:
                        tblDetalle.Connection.Open();
                        transaction = tblDetalle.Connection.BeginTransaction();
                        tblDetalle.Transaction = transaction;

                        Console.Write("Codigo: ");
                        codigo = Int32.Parse(Console.ReadLine());

                        Console.Write("Mensaje corto: ");
                        mensajeCorto = Console.ReadLine();

                        Console.Write("Mensaje: ");
                        mensaje = Console.ReadLine();

                        Console.Write("Enviado por: ");
                        enviadoPor = Console.ReadLine();

                        Console.Write("Para: ");
                        para = Console.ReadLine();

                        Console.Write("Relacion: ");
                        relacion = Int32.Parse(Console.ReadLine());

                        tblDetalle.ppInsertDetalle(codigo, mensajeCorto, mensaje, enviadoPor, para, relacion);
                        transaction.Commit();

                        tblPrincipal.Connection.Open();
                        transaction = tblPrincipal.Connection.BeginTransaction();
                        tblPrincipal.Transaction = transaction;
                        tblPrincipal.ppUpsertPrincipal(codigo, "vacio");
                        transaction.Commit();
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
