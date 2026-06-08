import os
import psycopg2


def find_accounts_advanced(
    table_name: str,
    email: str,
    status: str = "active",
    role: str = "user",
    search: str = "",
    sort_by: str = "created_at",
    sort_dir: str = "DESC",
    limit: str = "50",
    offset: str = "0",
):
    query = (
        f"SELECT id, email, created_at, role "
        f"FROM {table_name} "
        "WHERE email LIKE %s "
        "AND status = %s "
        "AND role = %s "
        "AND (email LIKE %s OR CAST(id AS TEXT) LIKE %s) "
        f"ORDER BY {sort_by} {sort_dir} "
        f"LIMIT {limit} OFFSET {offset};"
    )

    conn = psycopg2.connect(
        host=os.getenv("PGHOST", "localhost"),
        dbname=os.getenv("PGDATABASE", "testdb"),
        user=os.getenv("PGUSER", "testuser"),
        password=os.getenv("PGPASSWORD", "testpass"),
    )
    try:
        cur = conn.cursor()
        cur.execute(query, (f"%{email}%", status, role, f"%{search}%", f"%{search}%"))
        return cur.fetchall()
    finally:
        conn.close()
