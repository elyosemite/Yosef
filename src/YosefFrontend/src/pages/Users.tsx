import { useUsers } from "../hooks/useUsers";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent } from "@/components/ui/card";

export default function Users() {
  const { users, loading, error } = useUsers();

  if (loading) return <p>Load users...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="p-6">
      <Card className="shadow-lg">
        <CardContent>
          <h2 className="text-xl font-bold mb-4">Users</h2>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Name</TableHead>
                <TableHead>Status</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {users.map((user) => (
                <TableRow key={user.id}>
                  <TableCell className="text-left">{user.id}</TableCell>
                  <TableCell className="text-left">{user.name}</TableCell>
                  <TableCell className="text-left">
                    {user.active ? (
                      <span className="text-green-600">Ativo</span>
                    ) : (
                      <span className="text-red-600">Inativo</span>
                    )}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
