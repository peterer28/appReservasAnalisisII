from django.urls import path
from . import views

app_name = 'user_management'

urlpatterns = [
    path('register/', views.register_user, name='register'),  # Update the view reference
]