from django.shortcuts import render, redirect
from django.views.generic.edit import CreateView
from django.contrib.auth.forms import UserCreationForm
from django.urls import reverse_lazy
from .forms import UserRegistrationForm
from .models import CustomUser  # Import CustomUser model

def register_user(request):
    if request.method == 'POST':
        form = UserRegistrationForm(request.POST)
        if form.is_valid():
            # Extract the necessary fields
            form_data = form.cleaned_data
            username = form_data['username']
            password = form_data['password1']
            nombre = form_data['nombre']
            aMaterno = form_data['aMaterno']
            aPaterno = form_data['aPaterno']
            email = form_data['email']
            phone = form_data['phone']
            tipoUsuario = form_data['tipoUsuario']

            # Define a value for the missing 'usuario' argument
            usuario = "some_value_here"  # Replace with the appropriate value

            # Call the custom manager to create the user
            CustomUser.objects.create_user(
                username=username,
                password=password,
                nombre=nombre,
                aMaterno=aMaterno,
                aPaterno=aPaterno,
                email=email,
                phone=phone,
                tipoUsuario=tipoUsuario,
                usuario=usuario  # Provide the 'usuario' value here
            )

            return redirect('user_management:registration_success') # Define this URL in your app's URLs
    else:
        form = UserRegistrationForm()

    return render(request, 'user_management/register.html', {'form': form})

def registration_success(request):
    return render(request, 'user_management/registration_success.html')

