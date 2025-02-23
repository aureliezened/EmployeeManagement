CREATE OR REPLACE PROCEDURE add_employee(
  employee_id UUID,
  employee_data JSONB,
  attendance_data JSONB
) AS $$
DECLARE
  job_id INT; 
  status_id INT; 
  dep_id INT; 
  branch_id INT; 
  
BEGIN
  -- Find department ID based on department name
  SELECT "departmentId" INTO dep_id
  FROM "Departments"
  WHERE "departmentName" = employee_data->>'department';
  
  -- Find job ID based on job name
  SELECT "jobId" INTO job_id
  FROM "JobTitles"
  WHERE "jobName" = employee_data->>'jobTitle';
  
  -- Find status ID based on status name
  SELECT "statusId" INTO status_id
  FROM "EmployeeStatuses"
  WHERE "statusName" = employee_data->>'status';
  
  -- Find branch ID based on branch name
  SELECT "branchId" INTO branch_id
  FROM "Branches"
  WHERE "branchName" = employee_data->>'branch';
  
  -- Insert employee data into Employees table
  INSERT INTO "Employees"(
	"employeeId",
    "fullName",
    "email",
	"birthDate",
    "phoneNumber",
    "jobTitle",
    "department",
    "status",
	"branch",
    "joinedAt"
  ) VALUES (
	employee_id,
    employee_data->>'fullName',
    employee_data->>'email',
	(employee_data->>'birthDate')::DATE,
    employee_data->>'phoneNumber',
	job_id,
    dep_id,
    status_id,
    branch_id,
    now() ::DATE
  );
  -- Insert attendance data if provided
  IF attendance_data IS NOT NULL THEN
    INSERT INTO "Attendances" ("attendanceDate", "checkIn", "checkOut", "employeeId")
    SELECT
      (data ->> 'attendanceDate')::DATE,
      (data ->> 'checkIn')::TIME,
      (data ->> 'checkOut')::TIME,
      "employee_id"
    FROM jsonb_array_elements(attendance_data) AS data;
  END IF;
END;
$$ LANGUAGE plpgsql;