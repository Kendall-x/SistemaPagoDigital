# Sistema de Pago Digital

## Descripción del proyecto

Sistema de Pago Digital es una aplicación web desarrollada con ASP.NET Core MVC que permite gestionar cobros y pagos simulados entre usuarios con diferentes roles.

El sistema permite a los Clientes realizar recargas, consultar su saldo y pagar cobros mediante códigos de verificación. Los Conductores pueden generar cobros, consultar los pagos recibidos y visualizar reportes de sus ingresos.

## Objetivo general

Desarrollar un sistema web de pagos digitales que permita administrar cuentas, recargas, cobros y pagos simulados de manera organizada y segura.

## Funcionalidades principales

- Registro e inicio de sesión de usuarios.
- Administración de roles: Cliente y Conductor.
- Creación de cuentas y consulta de saldos.
- Recargas de saldo simulado.
- Historial de movimientos.
- Generación de cobros con códigos de verificación de seis dígitos.
- Expiración automática de cobros.
- Cancelación de cobros pendientes.
- Pago de cobros mediante códigos de verificación.
- Validación de pagos recibidos.
- Consulta de cobros por estado y fechas.
- Consulta de ingresos por día, semana y mes.
- Exportación de reportes a archivos CSV.

## Tecnologías utilizadas

- C#
- ASP.NET Core MVC
- .NET 10
- Entity Framework Core
- MySQL
- MySQL Workbench
- Razor Views (.cshtml)
- HTML
- CSS
- Bootstrap
- Git y GitHub
- Visual Studio

## Roles del sistema

### Cliente
- Consultar su cuenta y saldo.
- Realizar recargas simuladas.
- Consultar sus movimientos.
- Pagar cobros utilizando códigos de verificación.

### Conductor
- Generar cobros.
- Consultar pagos recibidos.
- Validar pagos.
- Cancelar cobros pendientes.
- Consultar reportes de cobros e ingresos.
- Exportar información a CSV.

## Requisitos para ejecutar el proyecto

- Visual Studio.
- .NET 10 SDK.
- MySQL Server.
- MySQL Workbench.
- Acceso al repositorio del proyecto.

## Configuración e instalación

1. Clonar el repositorio.
2. Abrir la solución en Visual Studio.
3. Configurar la conexión a MySQL mediante User Secrets o variables de entorno.
4. Restaurar los paquetes NuGet.
5. Ejecutar las migraciones de Entity Framework Core para crear la base de datos.
6. Ejecutar el proyecto desde Visual Studio.

## Base de datos

El sistema utiliza MySQL y Entity Framework Core.

Entre las principales entidades se encuentran:

- Usuarios
- Cuentas
- Recargas
- Cobros
- Pagos
- Movimientos

## Autores

- Kendall Salazar Chaves

## Uso de inteligencia artificial

Se utilizaron herramientas de inteligencia artificial como apoyo para la explicación de conceptos, revisión de errores, depuración y orientación durante el desarrollo, la que más use fue ChatGPT
