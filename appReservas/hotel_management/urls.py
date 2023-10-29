# hotel_management/urls.py
from django.urls import path
from . import views

app_name = 'hotel_management'

urlpatterns = [
    path('create/', views.create_hotel, name='create_hotel'),
    # Add other URLs as needed
]
