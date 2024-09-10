CALL add_employee(
  '223e4567-e89b-12d3-a456-426614174000',
  '{
    "fullName": "John Doe",
    "email": "johndoe@example.com",
    "birthDate": "1990-01-01",
    "phoneNumber": "123-456-7890",
    "jobTitle": "Back end developer",
    "department": "Development",
    "status": "Active",
    "branch": "Dubai",
    "joinedAt": "2024-07-15"
  }'::jsonb, -- employee_data (JSONB)
  NULL -- attendance_data (JSONB)
);
