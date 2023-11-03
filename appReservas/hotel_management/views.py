# hotel_management/views.py
from django.shortcuts import render, redirect
from .forms import HotelForm
import pyodbc

def create_hotel(request):
    if request.method == 'POST':
        form = HotelForm(request.POST)
        if form.is_valid():
            nombre = form.cleaned_data['nombre']
            direccion = form.cleaned_data['direccion']
            email = form.cleaned_data['email']
            phone = form.cleaned_data['phone']

            try:
                # Connect to your SQL Server database
                connection_string = "Driver={ODBC Driver 17 for SQL Server};Server=tcp:app-reservas-srv.database.windows.net,1433;Database=appReservasDB;Uid=maiz;Pwd=Pr1ngl3sdeBBQ;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;"
                connection = pyodbc.connect(connection_string)
                cursor = connection.cursor()

                # Call the stored procedure
                cursor.execute("EXEC sp_PopulateHotel ?, ?, ?, ?", (nombre, direccion, email, phone))
                connection.commit()

                # Close the database connection
                connection.close()

                # Redirect to a success page
                return render(request, 'hotel_management/success.html')

            except Exception as e:
                # Handle any exceptions here
                return render(request, 'hotel_management/error.html', {'error_message': str(e)})

    else:
        form = HotelForm()

    return render(request, 'hotel_management/create_hotel.html', {'form': form})

