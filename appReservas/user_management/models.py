import pyodbc
import hashlib
from django.contrib.auth.models import AbstractBaseUser, BaseUserManager, Group, Permission, PermissionsMixin
from django.db import models
from django.utils.translation import ugettext_lazy as _
from django.contrib.auth.models import BaseUserManager


class CustomUserManager(BaseUserManager):
    """
    Custom user manager where email is the unique identifiers
    for authentication instead of usernames.
    """

    def create_user(self, nombre, aMaterno, aPaterno, email, phone, usuario, password, tipoUsuario, **extra_fields):
        # ... previous code ...

        # Establish a database connection to your Azure SQL Database
        connection = None  # Initialize connection outside the try block

        try:
            # Specify your Azure SQL Database connection string
            connection_string = 'DRIVER={ODBC Driver 17 for SQL Server};' \
                                'SERVER=app-reservas-srv.database.windows.net;' \
                                'DATABASE=appReservasDB;' \
                                'UID=maiz;' \
                                'PWD=Pr1ngl3sdeBBQ;' \
                                'Encrypt=no;' \
                                'TrustServerCertificate=no;' \
                                'Connection Timeout=30;'

            # Establish a database connection
            connection = pyodbc.connect(connection_string)

            # Create a cursor for executing SQL commands
            cursor = connection.cursor()

            # Create a hash based on usuario and password
            password = hashlib.sha256(f"{usuario}{password}".encode()).hexdigest()

            # Execute the stored procedure
            cursor.execute("EXEC sp_CrearUsuario ?, ?, ?, ?, ?, ?, ?, ?", 
                           nombre, aMaterno, aPaterno, email, phone, usuario, password, tipoUsuario)

            # Commit the changes to the database
            connection.commit()
        except Exception as e:
            # Handle any exceptions, log errors, or raise them if needed
            raise e
        finally:
            if connection:
                connection.close()

    def create_superuser(self, email, password, **extra_fields):
        """
        Create and return a superuser with the given email and password.
        """
        extra_fields.setdefault('is_staff', True)
        extra_fields.setdefault('is_superuser', True)

        if extra_fields.get('is_staff') is not True:
            raise ValueError(_('Superuser must have is_staff=True.'))
        if extra_fields.get('is_superuser') is not True:
            raise ValueError(_('Superuser must have is_superuser=True.'))

        return self.create_user(email, password, **extra_fields)
    
class CustomUser(AbstractBaseUser, PermissionsMixin):
    email = models.EmailField(unique=True)
    nombre = models.CharField(max_length=30)
    aMaterno = models.CharField(max_length=30)
    aPaterno = models.CharField(max_length=30)
    phone = models.CharField(max_length=15)
    usuario = models.CharField(max_length=30, unique=True)
    tipoUsuario = models.CharField(max_length=20)
    
    is_active = models.BooleanField(default=True)
    is_staff = models.BooleanField(default=False)

    objects = CustomUserManager()

    # Add related_name to avoid clash
    groups = models.ManyToManyField(
        Group,
        verbose_name=_('groups'),
        blank=True,
        related_name='customuser_set',
        related_query_name='user'
    )
    user_permissions = models.ManyToManyField(
        Permission,
        verbose_name=_('user permissions'),
        blank=True,
        related_name='customuser_set',
        related_query_name='user'
    )

    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = ['nombre', 'aMaterno', 'aPaterno', 'phone', 'usuario', 'tipoUsuario']

    def __str__(self):
        return self.email